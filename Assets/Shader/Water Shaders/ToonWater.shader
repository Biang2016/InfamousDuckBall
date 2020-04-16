Shader "Roystan/Toon/Water"
{
    Properties
    {	
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725) // when our water is at its most shallow
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749) // when it is at its deepest
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1 // as a cutoff for the gradient—anything deeper than this will no longer change color
        _SurfaceNoise("Surface Noise", 2D) = "white" {} // add waves to the surface using a noise texture
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777 // threshold to get a more binary look
        _FoamMaxDistance("Foam Maximum Distance", Float) = 0.4 // Control for what depth the shoreline is visible
        _FoamMinDistance("Foam Minimum Distance", Float) = 0.04
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0) // control scroll speed, in UVs per second
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {} // Two channel distortion texture
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27 // Control to multiply the strength of the distortion.
        _FoamColor("Foam Color", Color) = (1,1,1,1) // control the color of the water foam
    }
    SubShader
    {
        Tags 
        {
            /*
            This tells Unity to render objects with this shader after all objects in the "Geometry" queue have been rendered; 
            this queue is usually where opaque objects are drawn. This way, we can overlay our transparent water on top of 
            all the opaque objects and blend them together
            */
            "Queue" = "Transparent"
        }
        Pass
        {
            /*
            how that blending should occur. We're using a blending algorithm often referred to as normal blending, 
            and is similar to how software like Photoshop blends two layers
            */
            Blend SrcAlpha OneMinusSrcAlpha 

            /*
            prevents our object from being written into the depth buffer; 
            if it was written into the depth buffer, it would completely occlude objects behind it, 
            instead of only partially obscuring them
            */
            ZWrite Off

			CGPROGRAM

            #define SMOOTHSTEP_AA 0.01

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;  

            float _DepthMaxDistance;
            sampler2D _CameraDepthTexture;

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            float _SurfaceNoiseCutoff;
            float _FoamMaxDistance;
            float _FoamMinDistance;
            float2 _SurfaceNoiseScroll;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            float _SurfaceDistortionAmount;

            sampler2D _CameraNormalsTexture;

            float4 _FoamColor;


            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0; // set up noise texture property
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD2; // calculate the screen space position of our vertex in the vertex shader
                float2 noiseUV : TEXCOORD0; // set up noise texture property
                float2 distortUV : TEXCOORD1;
                float3 viewNormal : NORMAL;
            };

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.screenPosition = ComputeScreenPos(o.vertex); // pass this value into the fragment shader where we can use it

                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);

                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);

                o.viewNormal = COMPUTE_VIEW_NORMAL;

                return o;
            }

            float4 frag (v2f i) : SV_Target 
            {
                /* sample the depth texture
                The first line will return the depth of the surface behind our water, in a range of 0 to 1. 
                This value is non-linear—one meter of depth very close to the camera will be 
                represented by a comparatively larger value in the depth texture than one meter a kilometer away from the camera.
                The second line converts the non-linear depth to be linear, in world units from the camera.
                */
                float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth01);

                /*
                Because what we care about is how deep this depth value is relative to our water surface, 
                we will also need to know the depth of the water surface. 
                */
                float depthDifference = existingDepthLinear - i.screenPosition.w;

                /*
                To calculate the color of our water, we're going to use the lerp function, 
                which takes two values (our two gradient colors in this case) and interpolates between them based on a third value in the 0 to 1 range. 
                Right now we have the depth in world units—instead we want to know how deep it is compared to our maximum depth, 
                percentage-wise. We can calculate this value by dividing depthDifference by our maximum depth. 
                */
                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);  // this function clamps the value between 0 and 1
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

                /*
                We declare our new texture property and add a new UV set as normal. In the fragment shader, 
                we sample the distortion texture—but before adding it to our noiseUV, we multiply it by 2 and subtract 1; 
                as a texture, the x and y values (red and green, respectively) are in the 0...1 range. As a two dimensional vector,
                however, we want it to be in the -1...1 range. The arithmetic below performs this operation.
                */
                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

                /*
                add motion by offsetting the UVs we use to sample the noise texture
                */
                float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);

                /*
                sample the noise texture and combine it with our surface color to render waves
                */
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

                /*
                The above codes vaguely resembles waves, but it's too smooth and has far too much variation 
                in brightness to match the toon style we're going for. We will apply a cutoff threshold to get a more binary look.
                Any values darker than the cutoff threshold are simply ignored, while any values above are drawn completely white
                */

                /*
                We'll need to calculate the view space normal of the water surface before we can compare it 
                to the normal rendered out to the texture. We can do this in the vertex shader and pass it through to the fragment shader.
                */

                float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));

                /*
                The dot product takes in two vectors (of any length) and returns a single number.
                When the vectors are parallel in the same direction and are unit vectors (vectors of length 1), this number is 1.
                When they are perpendicular, it returns 0. As you move a vector away from parallel—towards perpendicular—
                the dot product result will move from 1 to 0 non-linearly. 
                Note that when the angle between the vectors is greater than 90, the dot product will be negative.
                */
                float3 normalDot = saturate(dot(existingNormal, i.viewNormal));

                /*
                We'll use the result of the dot product to control the foam amount. When the dot product is large (near 1), 
                we'll use a lower foam threshold than when it is small (near 0).
                */

                /*
                We'd like the waves' intensity to increase near the shoreline or where objects intersect the surface of the water, 
                to create a foam effect. We'll achieve this effect by modulating the noise cutoff threshold based off the water depth.
                */

                float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot);
                float foamDepthDifference01 = saturate(depthDifference / foamDistance);

                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

                /*
                Unlike lerp, smoothstep is not linear: as the value moves from 0 to 0.5, it accelerates, 
                and as it moves from 0.5 to 1, it decelerates. This makes it ideal for smoothly blending values, 
                which is how we'll use it below.
                */
                float surfaceNoise = smoothstep(surfaceNoiseCutoff - SMOOTHSTEP_AA, surfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);

                float4 surfaceNoiseColor = _FoamColor * surfaceNoise;

                return waterColor + surfaceNoiseColor;
            }
            ENDCG
        }
    }
}
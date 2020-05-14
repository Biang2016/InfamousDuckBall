using UnityEngine;

public class AudioDuck : MonoBehaviour
{
    //The audio manager for FMOD is a bit different that standard Unity, but we don't do as much setting volume, pitch, etc (all of that is done in FMOD Studio)

    //A lot of the scripting techniques I use come from the Unity Example on FMOD's website: https://www.fmod.com/resources/documentation-unity?version=2.0&page=examples-basic.html

    //These strings are how Unity/FMOD look up the corresponding events in the sound banks

    [Header("BGM")] [FMODUnity.EventRef] public string BGM;

    [Header("Characters")]
    [FMODUnity.EventRef]
    public string DuckCharge, DuckFootsteps, DuckGenerateBuoy, DuckQuack, DuckTouchBuoy;

    [FMODUnity.EventRef] public string FishBreath, FishFlapping;

    [Header("SFX")] [FMODUnity.EventRef] public string BuoyInPlace, BuoyPop, Sea, Wind, Round, Score;
    [Header("Special")] [FMODUnity.EventRef] public string count321, count2;
    [Header("UI")] [FMODUnity.EventRef] public string Hover, Click, MenuBGM;

    FMOD.Studio.Bus MasterBus;

    //since this is a loop, we have a sound instance in our audio manager
    //FMOD.Studio.EventInstance playerMoveSoundInstance;
    public FMOD.Studio.EventInstance[] playerMoveSoundInstance = new FMOD.Studio.EventInstance[4];
    public FMOD.Studio.EventInstance[] playerQuackSoundInstance = new FMOD.Studio.EventInstance[4];
    public FMOD.Studio.EventInstance[] playerChargeSoundInstance = new FMOD.Studio.EventInstance[4];
    FMOD.Studio.EventInstance WindInstance;
    FMOD.Studio.EventInstance countdown321;
    FMOD.Studio.EventInstance countdown2;

    public static AudioDuck Instance;

    private void Awake()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        countdown321 = FMODUnity.RuntimeManager.CreateInstance(count321);
        countdown2 = FMODUnity.RuntimeManager.CreateInstance(count2);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    //this is our main way to play sounds in this game.  
    //Most non-loops can be played with PlayOneShotAttached, which attaches the fmod event to a game object and plays it.

    //this doesn't work if we have parameters though
    public void PlaySound(string soundToPlay, GameObject go)
    {
        //PlayOneShotAttached will play a sound and attach it to the specified transform

        FMODUnity.RuntimeManager.PlayOneShotAttached(soundToPlay, go);
    }

    public void PlaySpecial(FMOD.Studio.EventInstance specialsound)
    {
        specialsound.start();
    }

    public void StopSpecial(FMOD.Studio.EventInstance specialsound)
    {
        specialsound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void setProperty(FMOD.Studio.EVENT_PROPERTY index, float value)
    {
        //FMOD.Studio.EventInstance.setProperty(index, value);
        //FMODUnity.RuntimeManager.
    }

    //for sounds that have parameters, we need to keep track of the instance of the sound
    //we can attach a sound to an object with a rigidbody, and it will automatically update it's position and velocity
    public void StartPlayerMovementSound(PlayerNumber playernumber, Transform playerTransform, Rigidbody playerRB)
    {
        RaycastHit hit;

        //check to see if we already have a player move sound running (in which case we just update the parameter)
        if (!playerMoveSoundInstance[(int) playernumber].isValid())
        {
            if ((Physics.Raycast(playerTransform.position + playerTransform.up, Vector3.down, out hit)))
            {
                //first we make an instance of the sound
                playerMoveSoundInstance[(int) playernumber] = FMODUnity.RuntimeManager.CreateInstance(DuckFootsteps);
                //then attach that to the player object (now it will update position and velocity data)
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerMoveSoundInstance[(int) playernumber], playerTransform, playerRB);
                //bool a = hit.collider.gameObject.CompareTag("Metal");
                //if (a)
                //{
                //    playerMoveSoundInstance[(int)teamnumber].setParameterByName("On Metal", 1.0f);
                //}
                //else
                //{
                //    playerMoveSoundInstance[(int)teamnumber].setParameterByName("On Metal", 0.0f);
                //}
                //then we start it and release it (release means the instance will be destroyed when playback stops)
                playerMoveSoundInstance[(int) playernumber].start();
                playerMoveSoundInstance[(int) playernumber].release();
            }
        }
    }

    public void StartPlayerQuackSound(PlayerNumber playernumber, float teamnumber, float action, Transform playerTransform, Rigidbody playerRB)
    {
        //check to see if we already have a player move sound running (in which case we just update the parameter)
        if (!playerQuackSoundInstance[(int) playernumber].isValid())
        {
            //first we make an instance of the sound
            playerQuackSoundInstance[(int) playernumber] = FMODUnity.RuntimeManager.CreateInstance(DuckQuack);
            //then attach that to the player object (now it will update position and velocity data)
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerQuackSoundInstance[(int) playernumber], playerTransform, playerRB);
            //then we start it and release it (release means the instance will be destroyed when playback stops)
            playerQuackSoundInstance[(int) playernumber].start();
            playerQuackSoundInstance[(int) playernumber].release();
        }

        playerQuackSoundInstance[(int) playernumber].setParameterByName("Team", teamnumber);
        //action == 0 PUSH, action == 1 PULL
        playerQuackSoundInstance[(int) playernumber].setParameterByName("Action", action);
    }

    public void StartPlayerChargeSound(PlayerNumber playernumber, Transform playerTransform, Rigidbody playerRB)
    {
        //check to see if we already have a player move sound running (in which case we just update the parameter)
        if (!playerChargeSoundInstance[(int) playernumber].isValid())
        {
            //first we make an instance of the sound
            playerChargeSoundInstance[(int) playernumber] = FMODUnity.RuntimeManager.CreateInstance(DuckCharge);
            //then attach that to the player object (now it will update position and velocity data)
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerChargeSoundInstance[(int) playernumber], playerTransform, playerRB);
        }

        //then we start it and release it (release means the instance will be destroyed when playback stops)
        playerChargeSoundInstance[(int) playernumber].start();
        playerQuackSoundInstance[(int) playernumber].release();
    }

    public void StopPlayerChargeSound(PlayerNumber playernumber)
    {
        playerChargeSoundInstance[(int) playernumber].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StartWind(Transform windTransform, Rigidbody windRB)
    {
        //check to see if we already have a player move sound running (in which case we just update the parameter)
        if (!WindInstance.isValid())
        {
            //first we make an instance of the sound
            WindInstance = FMODUnity.RuntimeManager.CreateInstance(Wind);
            //then attach that to the player object (now it will update position and velocity data)
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(WindInstance, windTransform, windRB);
            //then we start it and release it (release means the instance will be destroyed when playback stops)
            WindInstance.start();
            WindInstance.release();
        }

        WindInstance.setParameterByName("wind", 1.0f);
    }

    public void StopWind()
    {
        WindInstance.setParameterByName("wind", 0.0f);
    }

    public void StopAllWOCEvents()
    {
        //BgmInstance.setParameterByName("Stop", 1.0f);
        //foreach (string s in Enum.GetNames(typeof(BattleTypes)))
        //{
        //    BgmInstance.setParameterByName(s, 0.0f);
        //}

        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    /*public void PlayButtonSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot(menuButtonSound);
	}*/
}
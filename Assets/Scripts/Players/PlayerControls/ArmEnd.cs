using UnityEngine;

public abstract class ArmEnd : Controllable
{
    public override void Initialize(PlayerControl parentPlayerControl)
    {
        base.Initialize(parentPlayerControl);
        foreach (Collider c in transform.GetComponentsInChildren<Collider>())
        {
            if (c.gameObject.layer == GameManager.Instance.Layer_BallKicker)
            {
                string layerName = "BallKicker" + ((int) (ParentPlayerControl.Player.PlayerInfo.PlayerNumber) + 1);
                int layer = LayerMask.NameToLayer(layerName);
                c.gameObject.layer = layer;
            }
        }
    }
}
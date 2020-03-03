using System;
using UnityEngine;

public abstract class Controllable : MonoBehaviour
{
    protected PlayerControl ParentPlayerControl;

    protected bool IsAI => ParentPlayerControl.Player.PlayerInfo.PlayerNumber == PlayerNumber.AI;

    public virtual void Initialize(PlayerControl parentPlayerControl)
    {
        ParentPlayerControl = parentPlayerControl;
    }

    protected virtual void FixedUpdate()
    {
        if (ParentPlayerControl.Controllable)
        {
            if (ParentPlayerControl.Player.PlayerInfo.PlayerNumber == PlayerNumber.AI)
            {
                Operate_AI();
            }
            else
            {
                if (MultiControllerManager.Instance.PlayerControlMap.ContainsKey(ParentPlayerControl.Player.PlayerInfo.PlayerNumber))
                {
                    PlayerNumber controllerIndex = MultiControllerManager.Instance.PlayerControlMap[ParentPlayerControl.Player.PlayerInfo.PlayerNumber];
                    Operate_Manual(controllerIndex);
                }
            }
        }
    }

    protected abstract void Operate_Manual(PlayerNumber controllerIndex);

    protected abstract void Operate_AI();
}
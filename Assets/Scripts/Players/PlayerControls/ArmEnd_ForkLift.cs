using UnityEngine;

public class ArmEnd_ForkLift : ArmEnd
{
    [SerializeField] private float LookAtMinDistance = 3f;
    [SerializeField] private Animator Anim;

    public float ResponseSpeed = 0.5f;

    public bool IsClamping = false;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger])
        {
            Anim.SetTrigger("Clamp");
        }

        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.RightTrigger])
        {
            IsClamping = false;
            Anim.SetTrigger("Release");
        }
    }

    private bool Hold = false;

    protected override void Operate_AI()
    {
    }

    void LateUpdate()
    {
        Quaternion targetRot = new Quaternion();

        if (IsClamping)
        {
            targetRot = Quaternion.LookRotation(transform.position - ParentPlayerControl.Player.GetPlayerPosition);
        }
        else
        {
            targetRot = Quaternion.LookRotation(GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position);
        }

        Quaternion r = Quaternion.Lerp(transform.rotation, targetRot, ResponseSpeed);
        transform.rotation = Quaternion.Euler(Vector3.Scale(new Vector3(0, 1, 1), r.eulerAngles));
    }

    public void OnHold()
    {
        IsClamping = true;
    }
}
using UnityEngine;

public class ArmEnd_ForkLift : ArmEnd
{
    [SerializeField] private float LookAtMinDistance = 3f;

    public float ResponseSpeed = 0.5f;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
    }

    private bool Hold = false;

    protected override void Operate_AI()
    {
    }

    void LateUpdate()
    {
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;
        if (diff.magnitude > LookAtMinDistance)
        {
            transform.LookAt(GameManager.Instance.Cur_BattleManager.Ball.transform);
        }

        //Quaternion rotation = Quaternion.LookRotation(GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position);
        //Quaternion r = Quaternion.Lerp(transform.rotation, rotation, ResponseSpeed);
        transform.rotation = Quaternion.Euler(Vector3.Scale(new Vector3(0, 1, 1), transform.rotation.eulerAngles));
    }
}
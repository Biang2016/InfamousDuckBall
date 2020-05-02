public class AudioAPI
{
    void Examples()
    {
        //玩家走路的音效调用
        //AudioDuck.Instance.StartPlayerMovementSound(playernumber, playerTransform, playerRigidbody);
        //鸭叫音效调用
        //AudioDuck.Instance.StartPlayerQuackSound(playernumber,teamnumber,action, playerTransform, playerRB);
        //蓄力音效调用
        //AudioDuck.Instance.StartPlayerChargeSound(playernumber, Transform playerTransform, Rigidbody playerRB);
        //蓄力音效停用
        //AudioDuck.Instance.StopPlayerChargeSound(playernumber);

        //调用音效
        //AudioDuck.Instance.PlaySound(AudioDuck.Instance.sound, gameObject);

        //开始刮风
        //AudioDuck.Instance.StartWind(windTransform, windRB);
        //停止刮风
        //AudioDuck.Instance.StopWind();

        //生成救生圈——DuckGenerateBuoy，鸭子碰到救生圈——DuckTouchBuoy;
        //河豚被捅——FishBreath, 河豚上岸摆动——FishFlapping;
        //放救生圈得分——BuoyInPlace, 救生圈爆炸——BuoyPop, 大海——Sea;
    }
}
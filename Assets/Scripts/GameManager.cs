using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private DebugPanel debugPanel;

    private Vector3 BallDefaultPos = Vector3.zero;
    public float PlayerRadius;

    void Start()
    {
        debugPanel = UIManager.Instance.ShowUIForms<DebugPanel>();
        debugPanel.SetScore(0, 0);
        UIManager.Instance.ShowUIForms<CameraDividePanel>();

        Player1.Initialize();
        Player2.Initialize();

        BallDefaultPos = Ball.transform.position;
        Input.ResetInputAxes();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F10))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public Player Player1;
    public Player Player2;

    public GoalBall Ball;

    public void Score(PlayerNumber playerNumber)
    {
        switch (playerNumber)
        {
            case PlayerNumber.P1:
            {
                Player2.Score++;
                Player1.ParticleSystem.Play();
                break;
            }
            case PlayerNumber.P2:
            {
                Player1.Score++;
                Player2.ParticleSystem.Play();
                break;
            }
        }

        debugPanel.SetScore(Player1.Score, Player2.Score);
        ResetBall();
    }

    public void ResetBall()
    {
        Ball.transform.position = BallDefaultPos;
        Ball.Reset();
    }

    [SerializeField] private BoxCollider Boundary;

    public float X_Min => Boundary.bounds.center.x - Boundary.bounds.extents.x;
    public float X_Max => Boundary.bounds.center.x + Boundary.bounds.extents.x;
    public float Z_Min => Boundary.bounds.center.z - Boundary.bounds.extents.z;
    public float Z_Max => Boundary.bounds.center.z + Boundary.bounds.extents.z;
}

public enum JoystickAxis
{
    None = 0,
    L_H = 1,
    L_V = 2,
    R_H = 3,
    R_V = 4,
    Trigger = 5,
    DH = 6,
    DV = 7,
}

public enum JoystickButton
{
    None = 0,
    LB = 1,
    RB = 2,
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    void Start()
    {
        UIManager.Instance.ShowUIForms<DebugPanel>();
        UIManager.Instance.ShowUIForms<CameraDividePanel>();

        Player1.Initialize();
        Player2.Initialize();
    }

    public void Reset()
    {
        SceneManager.LoadScene("MainScene");
    }

    public Player Player1;
    public Player Player2;

    public GameObject Ball;
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
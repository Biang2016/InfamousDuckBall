using System;
using System.Collections.Generic;
using System.Linq;

public class MultiControllerManager : MonoSingleton<MultiControllerManager>
{
    public Dictionary<PlayerNumber, PlayerNumber> PlayerControlMap = new Dictionary<PlayerNumber, PlayerNumber>(); // Key: playerNumber , Value: controller

    public Dictionary<PlayerNumber, Controller> Controllers = new Dictionary<PlayerNumber, Controller>();

    void Awake()
    {
        foreach (object o in Enum.GetValues(typeof(PlayerNumber)))
        {
            PlayerNumber pn = (PlayerNumber) o;
            if ((int) pn < GameManager.MaximalPlayerNumber)
            {
                Controllers.Add(pn, new XBoxController());
                Controllers[pn].Init(pn);
            }

            if ((int) pn == 4)
            {
                Controllers.Add(pn, new KeyBoardController());
                Controllers[pn].Init(pn);
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (KeyValuePair<PlayerNumber, Controller> kv in Controllers)
        {
            kv.Value.FixedUpdate();
        }

        foreach (object o in Enum.GetValues(typeof(PlayerNumber)))
        {
            PlayerNumber pn = (PlayerNumber) o;
            if ((int) pn <= GameManager.MaximalPlayerNumber)
            {
                if (Controllers[pn].AnyButtonPressed())
                {
                    if (PlayerObjectRegistry.MyPlayer)
                    {
                        if (!PlayerControlMap.ContainsKey(PlayerObjectRegistry.MyPlayer.PlayerNumber))
                        {
                            PlayerControlMap.Add(PlayerObjectRegistry.MyPlayer.PlayerNumber, pn);
                        }
                    }
                }
            }
        }
    }
}
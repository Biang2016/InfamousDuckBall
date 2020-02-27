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
            if ((int) pn < 2)
            {
                Controllers.Add(pn, new XBoxController());
                Controllers[pn].Init((int) pn + 1);
            }
        }
    }

    void Update()
    {
        foreach (KeyValuePair<PlayerNumber, Controller> kv in Controllers)
        {
            kv.Value.Update();
        }

        foreach (object o in Enum.GetValues(typeof(PlayerNumber)))
        {
            PlayerNumber pn = (PlayerNumber) o;
            if ((int) pn < 3)
            {
                if (Controllers[pn].AnyButtonPressed())
                {
                    if (!PlayerControlMap.Values.ToList().Contains(pn))
                    {
                        if (PlayerControlMap.Count < 2)
                        {
                            PlayerControlMap.Add((PlayerNumber) PlayerControlMap.Count, pn);
                        }
                    }
                }
            }
        }
    }
}
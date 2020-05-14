using System.Collections.Generic;
using Bolt;
using UdpKit;
using UnityEngine;

[BoltGlobalBehaviour]
public class NetworkCallbacks : GlobalEventListener
{
    List<string> logMessages = new List<string>();

    void OnGUI()
    {
        int maxMessages = Mathf.Min(5, logMessages.Count);
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height - 100, 400, 100), GUI.skin.box);
        for (int i = 0; i < maxMessages; ++i)
        {
            GUILayout.Label(logMessages[i]);
        }

        GUILayout.EndArea();
    }

    public override void OnEvent(LogEvent evnt)
    {
        logMessages.Insert(0, evnt.Message);
    }

    public override void ConnectRequest(UdpEndPoint endpoint, IProtocolToken token)
    {
        BoltManager.ConnectRequest_Server(endpoint, token);
    }
}
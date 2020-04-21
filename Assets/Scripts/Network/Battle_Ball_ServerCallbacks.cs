using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "Battle_FlagRace", "Battle_Smash")]
public class Battle_Ball_ServerCallbacks : Bolt.GlobalEventListener
{
    void Awake()
    {
    }

    public override void Connected(BoltConnection connection)
    {
    }

    public override void SceneLoadLocalDone(string map)
    {
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
    }
}
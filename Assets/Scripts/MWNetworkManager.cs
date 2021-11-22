using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MWNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("Connecter to server!");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MWNetworkPlayer player = conn.identity.GetComponent<MWNetworkPlayer>();

        player.SetDisplayName($"Player {numPlayers}");

        Color displayColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));

        player.SetDisplayColor(displayColor);
        Debug.Log($"There are now {numPlayers} players");
    }
}

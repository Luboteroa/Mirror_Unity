using UnityEngine;
using Mirror;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawnPrefab;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject unitSpawnerInstance = Instantiate(
            unitSpawnPrefab, 
            conn.identity.transform.position, 
            conn.identity.transform.rotation);
        
        NetworkServer.Spawn(unitSpawnerInstance, conn);
    }
    
}

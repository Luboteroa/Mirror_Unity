using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private Health _health;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitSpawnPoint;

    #region Server

    public override void OnStartServer()
    {
        _health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        _health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(
            unitPrefab,
            unitSpawnPoint.position, 
            unitSpawnPoint.rotation);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region  Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) {return;}

        if(!hasAuthority){return;}

        CmdSpawnUnit();
    }

    #endregion
}

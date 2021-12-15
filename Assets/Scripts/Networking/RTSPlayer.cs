using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();
    public List<Unit> GetMyUnits() 
    {
        return myUnits;
    }

    #region Server
    public override void OnStartAuthority()
    {
        if(NetworkServer.active) { return; }
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        if(!isClientOnly || !hasAuthority) { return; }
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
    }

    private void ServerHandleUnitSpawned(Unit unit)
    {
        myUnits.Add(unit);
    }

    private void ServerHandleUnitDespawned(Unit unit)
    {
        myUnits.Remove(unit);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if(!isClientOnly) { return; }

        Unit.AutorithyOnUnitSpawned += AutorithyHandleUnitSpawned;
        Unit.AutorithyOnUnitDespawned += AutorithyHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        if(!isClientOnly) { return; }

        Unit.AutorithyOnUnitSpawned -= AutorithyHandleUnitSpawned;
        Unit.AutorithyOnUnitDespawned -= AutorithyHandleUnitDespawned;
    }

    private void AutorithyHandleUnitSpawned(Unit unit)
    {
        if(!hasAuthority) {return;}

        myUnits.Add(unit);
    }

    private void AutorithyHandleUnitDespawned(Unit unit)
    {
        if(!hasAuthority) {return;}

        myUnits.Remove(unit);
    }


    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class Unit : NetworkBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private UnitMovement _unitMovement;
    [SerializeField] private UnityEvent _onSelected;
    [SerializeField] private UnityEvent _onDeselected;
    [SerializeField] private Targeter _targeter;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AutorithyOnUnitSpawned;
    public static event Action<Unit> AutorithyOnUnitDespawned;


    public UnitMovement GetUnitMovement()
    {
        return _unitMovement;
    }

    public Targeter GetTargeter()
    {
        return _targeter;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
        _health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
        _health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    
    #endregion

    #region Client

    public override void OnStartAuthority()
    {      
        AutorithyOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if(!hasAuthority) { return; }

        AutorithyOnUnitDespawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if(!hasAuthority){return;}
        _onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if(!hasAuthority){return;}

        _onDeselected?.Invoke();
    }

    #endregion
}

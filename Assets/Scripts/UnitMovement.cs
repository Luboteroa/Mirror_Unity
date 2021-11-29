using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Camera _mainCamera;

    #region Server

    [Command]
    private void CmdMove(Vector3 position)
    {
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        _agent.SetDestination(hit.position);
    }

    #endregion

    #region  Client

    public override void OnStartAuthority()
    {
        _mainCamera = Camera.main;
    }
    
    [ClientCallback]
    private void Update()
    {
        if(!hasAuthority) {return;}

        if(!Mouse.current.rightButton.wasPressedThisFrame) {return;}

        Ray _ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(!Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity)) {return;}

        CmdMove(hit.point);
    }

    #endregion
}

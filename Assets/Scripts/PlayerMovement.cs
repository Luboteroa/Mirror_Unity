using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class PlayerMovement : NetworkBehaviour
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

        if(!Input.GetMouseButton(1)) {return;}

        Ray _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity)) {return;}

        CmdMove(hit.point);
    }

    #endregion
}

using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        _agent.SetDestination(hit.position);
    }

    #endregion
}

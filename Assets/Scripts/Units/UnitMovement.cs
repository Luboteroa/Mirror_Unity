using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Targeter _targeter;
    [SerializeField] private float chaseRange = 10f;

    [ServerCallback]
    private void Update()
    {
        Targetable _target = _targeter.GetTargeter();
        if(_target != null)
        {
            if ((_target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange)
            {
                //chase
                _agent.SetDestination(_target.transform.position);
            }
            else if(_agent.hasPath)
            {
                //stop
                _agent.ResetPath();
            }
            return;
        }
        if (!_agent.hasPath) { return; }
        
        if(_agent.remainingDistance > _agent.stoppingDistance) { return; }

        _agent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        _targeter.ClearTarget();
        
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        _agent.SetDestination(hit.position);
    }
}

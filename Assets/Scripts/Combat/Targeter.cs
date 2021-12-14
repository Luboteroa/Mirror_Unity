using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{
    [SerializeField] private Targetable _target;

    public Targetable GetTargeter()
    {
        return _target;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget)){ return; }

        _target = newTarget;
    }

    [Server]
    public void ClearTarget()
    {
        _target = null;
    }
}

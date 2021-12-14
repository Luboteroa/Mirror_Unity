using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int damageToDeal = 20;
    [SerializeField] private float destroyAfterSeconds = 5f;
    [SerializeField] private float launchForce = 10f;

    private void Start()
    {
        rb.velocity = transform.forward * launchForce;
    } 

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NetworkIdentity>(out NetworkIdentity _networkIdentity))
        {
            if(_networkIdentity.connectionToClient == connectionToClient){ return; }
        }

        if(other.TryGetComponent<Health>(out Health _health))
        {
            _health.DealDamage(damageToDeal);
        }
        DestroySelf();
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

}

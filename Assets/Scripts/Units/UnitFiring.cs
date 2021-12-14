using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter _targeter;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform ProjectileSpawnPoint;
    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 20f;

    private float lastFiretime;

    [ServerCallback]
    private void Update()
    {
        Targetable _target = _targeter.GetTargeter();

        if(_target == null) { return; }

        if(!CanFireAtTarget()) { return; }

        Quaternion targetRotation = 
            Quaternion.LookRotation(_target.transform.position - transform.position);
        
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if(Time.time > 1 / fireRate + lastFiretime)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(
                _target.GetAimPoint().position - ProjectileSpawnPoint.position);

            GameObject projectileInstance = Instantiate(
                ProjectilePrefab, ProjectileSpawnPoint.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFiretime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (_targeter.GetTargeter().transform.position - transform.position).sqrMagnitude
            <= fireRange * fireRange;

    }
}

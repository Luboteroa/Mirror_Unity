using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targetable : NetworkBehaviour
{
    [SerializeField] private Transform aimAtPoint;

    public Transform GetAimPoint()
    {
        return aimAtPoint;
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityCommandGiver : MonoBehaviour
{
    [SerializeField] private LayerMask _layer = new LayerMask();
    [SerializeField] private UnitSelectionHandler _unitSelectionHandler;
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        if(!Mouse.current.rightButton.wasPressedThisFrame) {return;}

        Ray _ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(!Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity)) {return;}

        if(hit.collider.TryGetComponent<Targetable>(out Targetable _target))
        {
            if(_target.hasAuthority)
            {
                TryMove(hit.point);
                return;
            }

            TryTarget(_target);
            return;
        }

        TryMove(hit.point);
    }

    private void TryMove(Vector3 point)
    {
         foreach(Unit _unit in _unitSelectionHandler._selectedUnits)
         {
            _unit.GetUnitMovement().CmdMove(point);
         }
    }

    private void TryTarget(Targetable _target)
    {
         foreach(Unit _unit in _unitSelectionHandler._selectedUnits)
         {
            _unit.GetTargeter().CmdSetTarget(_target.gameObject);
         }
    }
}

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

        TryMove(hit.point);
    }

    private void TryMove(Vector3 point)
    {
         foreach(Unit _unit in _unitSelectionHandler._selectedUnits)
         {
            _unit.GetUnitMovement().CmdMove(point);
         }
    }
}

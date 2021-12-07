using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform UnitSelectionArea;
    [SerializeField] private LayerMask _layer = new LayerMask();

    private Vector2 startPosition;
    private Camera _camera;
    private RTSPlayer player;
    public List<Unit> _selectedUnits { get; } = new List<Unit>();

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }

        else if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }

        else if(Mouse.current.leftButton.isPressed)
        {
           UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    {
        if(!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (Unit selectedUnit in _selectedUnits)
            {
                selectedUnit.Deselect();
            }
            _selectedUnits.Clear();
        }

        UnitSelectionArea.gameObject.SetActive(true);

        startPosition = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }
    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight= mousePosition.y - startPosition.y;

        UnitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        UnitSelectionArea.anchoredPosition = startPosition +
            new Vector2(areaWidth/2, areaHeight/2);
    }

    private void ClearSelectionArea()
    {
        UnitSelectionArea.gameObject.SetActive(false);

        if(UnitSelectionArea.sizeDelta.magnitude == 0)
        {
            Ray _ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if(!Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity, _layer)){return;}

            if(!hit.collider.TryGetComponent<Unit>(out Unit _unit)) {return;}

            if(!_unit.hasAuthority) {return;}

            _selectedUnits.Add(_unit);

            foreach (Unit selectedUnit in _selectedUnits)
            {
                selectedUnit.Select();
            }

            return;
        }

        Vector2 min = UnitSelectionArea.anchoredPosition - (UnitSelectionArea.sizeDelta/2);
        Vector2 max = UnitSelectionArea.anchoredPosition + (UnitSelectionArea.sizeDelta/2);

        foreach(Unit unit in player.GetMyUnits())
        {
            if (_selectedUnits.Contains(unit)) { continue; }
            
            Vector3 screenPosition = _camera.WorldToScreenPoint(unit.transform.position);

            if(screenPosition.x > min.x &&
                screenPosition.x < max.x &&
                screenPosition.y > min.y &&
                screenPosition.y < max.y)
                {
                    _selectedUnits.Add(unit);
                    unit.Select();
                }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            bool clickedOnUnit = false;

            foreach (RaycastHit hit in hits)
            {
                SelectableUnit unit = hit.collider.GetComponent<SelectableUnit>();
                if (unit != null)
                {
                    clickedOnUnit = true;
                    SelectUnit(unit);
                    break;
                }
            }

            if (!clickedOnUnit)
            {
                DeselectAllUnits();
            }
        }
    }

    public void SelectUnit(SelectableUnit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.Select();
        }
    }

    public void DeselectUnit(SelectableUnit unit)
    {
        if (selectedUnits.Contains(unit))
        {
            selectedUnits.Remove(unit);
            unit.Deselect();
        }
    }

    public void ToggleUnitSelection(SelectableUnit unit)
    {
        if (selectedUnits.Contains(unit))
        {
            DeselectUnit(unit);
        }
        else
        {
            SelectUnit(unit);
        }
    }

    public void DeselectAllUnits()
    {
        foreach (var unit in selectedUnits)
        {
            if (unit != null)
                unit.Deselect();
        }
        selectedUnits.Clear();
    }

    public bool IsUnitSelected(SelectableUnit unit)
    {
        return selectedUnits.Contains(unit);
    }

    public List<SelectableUnit> GetSelectedUnits()
    {
        return selectedUnits;
    }

    public Dictionary<UnitType, List<SelectableUnit>> GetSelectedUnitsByType()
    {
        Dictionary<UnitType, List<SelectableUnit>> grouped = new Dictionary<UnitType, List<SelectableUnit>>();

        foreach (SelectableUnit unit in selectedUnits)
        {
            if (!grouped.ContainsKey(unit.unitType))
            {
                grouped[unit.unitType] = new List<SelectableUnit>();
            }
            grouped[unit.unitType].Add(unit);
        }

        return grouped;
    }
}

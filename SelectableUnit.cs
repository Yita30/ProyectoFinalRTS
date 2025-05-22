using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    public UnitType unitType; // Elegir el tipo en el Inspector

    private Renderer unitRenderer;
    private Color originalColor;
    private bool isSelected = false;

    void Start()
    {
        unitRenderer = GetComponent<Renderer>();
        if (unitRenderer != null)
        {
            originalColor = unitRenderer.material.color;
        }
    }

    void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            UnitSelectionManager.Instance.ToggleUnitSelection(this);
        }
        else
        {
            UnitSelectionManager.Instance.DeselectAllUnits();
            UnitSelectionManager.Instance.SelectUnit(this);
        }
    }

    public void Select()
    {
        if (isSelected) return;

        isSelected = true;
        if (unitRenderer != null)
            unitRenderer.material.color = Color.green;
    }

    public void Deselect()
    {
        if (!isSelected) return;

        isSelected = false;
        if (unitRenderer != null)
            unitRenderer.material.color = originalColor;
    }

    void OnDestroy()
    {
        if (UnitSelectionManager.Instance != null)
        {
            UnitSelectionManager.Instance.DeselectUnit(this);
        }
    }
}

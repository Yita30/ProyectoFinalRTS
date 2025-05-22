using UnityEngine;

public class SpawnOnSelect : MonoBehaviour
{
    [Header("Prefabs de unidades")]
    public GameObject unitPrefab_G;
    public GameObject unitPrefab_H;
    public GameObject unitPrefab_J;

    [Header("Costos de unidades")]
    public int cost_G = 30;
    public int cost_H = 50;
    public int cost_J = 80;

    [Header("Configuración de spawn")]
    public Transform spawnPoint;
    public int maxUnits = 10;

    private int currentUnits = 0;
    private bool isSelected = false;

    void Update()
    {
        if (UnitSelectionManager.Instance != null)
        {
            isSelected = UnitSelectionManager.Instance.IsUnitSelected(GetComponent<SelectableUnit>());
        }

        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.G))
                TrySpawnUnit(unitPrefab_G, cost_G);

            if (Input.GetKeyDown(KeyCode.H))
                TrySpawnUnit(unitPrefab_H, cost_H);

            if (Input.GetKeyDown(KeyCode.J))
                TrySpawnUnit(unitPrefab_J, cost_J);
        }
    }

    void TrySpawnUnit(GameObject prefab, int cost)
    {
        if (prefab == null || currentUnits >= maxUnits) return;

        if (PlayerResources.Instance.HasEnoughGold(cost))
        {
            GameObject unit = Instantiate(prefab, spawnPoint != null ? spawnPoint.position : transform.position + Vector3.forward, Quaternion.identity);

            Unit unitScript = unit.GetComponent<Unit>();
            if (unitScript != null)
            {
                unitScript.mySpawner = this;
            }

            PlayerResources.Instance.SpendGold(cost);
            currentUnits++;
        }
        else
        {
            Debug.Log("No hay suficiente oro para crear esta unidad.");
        }
    }

    public void NotifyUnitDestroyed()
    {
        currentUnits = Mathf.Max(0, currentUnits - 1);
    }
}

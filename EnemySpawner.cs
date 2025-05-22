using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración base enemiga")]
    public GameObject unitPrefab;     // Prefab de la unidad enemiga
    public Transform spawnPoint;      // Punto de aparición
    public float spawnInterval = 5f;  // Tiempo entre unidades
    public int maxUnits = 15;         // Límite máximo de unidades

    private float timer = 0f;
    private int currentUnits = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentUnits < maxUnits)
        {
            SpawnUnit();
            timer = 0f;
        }
    }

    void SpawnUnit()
    {
        if (unitPrefab == null) return;

        GameObject unit = Instantiate(unitPrefab, spawnPoint != null ? spawnPoint.position : transform.position + Vector3.forward, Quaternion.identity);

        Unit unitScript = unit.GetComponent<Unit>();
        if (unitScript != null)
        {
            unitScript.mySpawner = this;
        }

        currentUnits++;
    }

    public void NotifyUnitDestroyed()
    {
        currentUnits = Mathf.Max(0, currentUnits - 1);
    }
}
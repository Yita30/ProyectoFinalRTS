using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMoveWithAreaDamage : MonoBehaviour
{
    public float attackRange = 2f;
    public float damageAmount = 20f;

    public float areaDamage = 20f;
    public float areaRadius = 3f;
    public float proximityRange = 5f;
    public float damageInterval = 5f;

    private NavMeshAgent agent;
    private SelectableUnit selectableUnit;
    private Vector3 targetPosition;
    private HealthSystem currentTarget;
    private float lastAreaDamageTime = -Mathf.Infinity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        selectableUnit = GetComponent<SelectableUnit>();
    }

    void Update()
    {
        // Movimiento por clic derecho
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (UnitSelectionManager.Instance != null && UnitSelectionManager.Instance.IsUnitSelected(selectableUnit))
                {
                    HealthSystem targetHealth = hit.collider.GetComponent<HealthSystem>();
                    if (targetHealth != null)
                    {
                        currentTarget = targetHealth;
                        targetPosition = hit.transform.position;
                        StartCoroutine(AttackTarget(targetHealth, targetPosition));
                    }
                    else
                    {
                        currentTarget = null;
                        agent.SetDestination(hit.point);
                    }
                }
            }
        }

        //  SIEMPRE busca si hay enemigos cerca para hacer daño en área
        if (Time.time >= lastAreaDamageTime + damageInterval)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, proximityRange);
            foreach (var hit in hits)
            {
                if (hit.transform == transform) continue;

                HealthSystem health = hit.GetComponent<HealthSystem>();
                if (health != null)
                {
                    DoAreaDamage();
                    lastAreaDamageTime = Time.time;
                    break; // solo aplicar una vez cada intervalo
                }
            }
        }
    }


    private IEnumerator AttackTarget(HealthSystem target, Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);

        while (target != null && Vector3.Distance(transform.position, targetPosition) > attackRange)
        {
            yield return null;
        }

        if (target != null)
        {
            target.TakeDamage(damageAmount);
            Debug.Log($"{gameObject.name} atacó a {target.gameObject.name}");
        }
    }

    private void DoAreaDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, areaRadius);
        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;
            
            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(areaDamage);
                Debug.Log($"{gameObject.name} hizo daño en área a {hit.name}");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, proximityRange);
    }
}
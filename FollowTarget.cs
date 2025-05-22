using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowTarget : MonoBehaviour
{
    public float detectionRange = 10f;
    public float areaDamage = 20f;
    public float areaRadius = 3f;
    public float proximityRange = 5f;
    public float damageInterval = 5f;

    public Transform playerBase; // Asigna la base del jugador desde el Inspector

    private NavMeshAgent agent;
    private Transform currentTarget;
    private float lastDamageTime = -Mathf.Infinity;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating(nameof(FindClosestTarget), 0f, 0.5f);
    }

    void Update()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);

            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= proximityRange && Time.time >= lastDamageTime + damageInterval)
            {
                DoAreaDamage();
                lastDamageTime = Time.time;
            }
        }
    }

    void FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float shortestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("PlayerUnit")) // O el tag que uses para los jugadores
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    closest = hit.transform;
                }
            }
        }

        if (closest != null)
        {
            currentTarget = closest;
        }
        else if (playerBase != null)
        {
            currentTarget = playerBase;
        }
    }

    void DoAreaDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, areaRadius);
        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;

            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(areaDamage);
                Debug.Log($"{name} hizo daño en área a {hit.name}");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, proximityRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

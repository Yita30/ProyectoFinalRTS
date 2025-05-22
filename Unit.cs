using UnityEngine;
using UnityEngine.AI;

public enum UnitType
{
    Rock,
    Paper,
    Scissors
}

public class Unit : MonoBehaviour
{
    public MonoBehaviour mySpawner;
    public UnitType unitType;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float attackDamage = 10f;

    private float lastAttackTime = 0f;
    private NavMeshAgent agent;
    private Transform target;

    private string enemyTag;
    private string enemyBaseTag;

    void Start()
    {
        

        if (CompareTag("PlayerUnit"))
        {
            enemyTag = "EnemyUnit";
            enemyBaseTag = "EnemyBase";
        }
        else if (CompareTag("EnemyUnit"))
        {
            enemyTag = "PlayerUnit";
            enemyBaseTag = "PlayerBase";
        }
        else
        {
            Debug.LogWarning($"{name} no tiene tag válido: PlayerUnit o EnemyUnit");
        }

        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f); // Buscar objetivos cada 0.5 segundos
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);

            float dist = Vector3.Distance(transform.position, target.position);
            if (dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackTarget();
                lastAttackTime = Time.time;
            }
        }
    }


    void UpdateTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float shortestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;

            if (hit.CompareTag(enemyTag) || hit.CompareTag(enemyBaseTag))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestTarget = hit.transform;
                }
            }
        }

        target = closestTarget;
    }

    void AttackTarget()
    {
        if (target == null) return;

        HealthSystem health = target.GetComponent<HealthSystem>();
        if (health == null) return;

        float finalDamage = attackDamage;

        Unit enemyUnit = target.GetComponent<Unit>();
        if (enemyUnit != null)
        {
            if (IsStrongAgainst(enemyUnit.unitType)) finalDamage *= 1.5f;
            else if (IsWeakAgainst(enemyUnit.unitType)) finalDamage *= 0.5f;
        }

        health.TakeDamage(finalDamage);
        Debug.Log($"{name} atacó a {target.name} con {finalDamage} de daño");
    }

    bool IsStrongAgainst(UnitType other)
    {
        return (unitType == UnitType.Rock && other == UnitType.Scissors) ||
               (unitType == UnitType.Paper && other == UnitType.Rock) ||
               (unitType == UnitType.Scissors && other == UnitType.Paper);
    }

    bool IsWeakAgainst(UnitType other)
    {
        return (unitType == UnitType.Rock && other == UnitType.Paper) ||
               (unitType == UnitType.Paper && other == UnitType.Scissors) ||
               (unitType == UnitType.Scissors && other == UnitType.Rock);
    }

    void OnDestroy()
    {
        if (mySpawner != null)
        {
            if (mySpawner is SpawnOnSelect playerSpawner)
                playerSpawner.NotifyUnitDestroyed();
            else if (mySpawner is EnemySpawner enemySpawner)
                enemySpawner.NotifyUnitDestroyed();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

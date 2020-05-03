using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Patrol,
        Chase,
        Attack,
    }

    public FSMStates currentState;
    public GameObject explosionParticles;

    public float attackDistance = 5;
    public float chaseDistance = 10;
    public float enemySpeed = 5;
    public GameObject player;

    public NavMeshAgent agent;
    public Transform enemyEyes;
    public float fieldOfView = 45f;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    float distanceToPlayer;

    int health;
    int currentDestinationIndex = 0;
    Transform deadTransform;

    bool attacking = false;

    void Start()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        player = GameObject.FindGameObjectWithTag("Player");

        Initialize();
        attacking = false;
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, 
            player.transform.position);


        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
        }
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;

        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        agent.stoppingDistance = 0;

        if(Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if(IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);

    }

    void UpdateChaseState()
    {
        nextDestination = player.transform.position;

        agent.stoppingDistance = attackDistance;

        agent.speed = 5;

        if(distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if(distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);
        
        transform.position = Vector3.MoveTowards
            (transform.position, nextDestination, enemySpeed * Time.deltaTime);

    }

    void UpdateAttackState()
    {
        print("attack");

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        if (!attacking)
        {
            StartCoroutine(EnemyAttacking());
        }
    }

    IEnumerator EnemyAttacking()
    {
        attacking = true;
        Instantiate(explosionParticles, transform.position, transform.rotation);
        int randomInt = (int) System.Math.Ceiling(Random.Range(3f, 6f));
        player.GetComponent<PlayerHealth>().TakeDamage(randomInt);
        yield return new WaitForSeconds(0.4f);
        attacking = false;
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) 
            % wanderPoints.Length;

        agent.SetDestination(nextDestination);

    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp
            (transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * frontRayPoint;
        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if(Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    print("Player in sight!");
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            player.GetComponent<PlayerHealth>().TickUpScore();
            Die();
        }
    }

    private void Die()
    {
        Instantiate(explosionParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

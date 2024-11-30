using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patroling,
        Chasing,
        Attacking,
        Dead

    }
    public PlayerValues enemyCount;
    public EnemyState currentState;
    public float attackRadius = 2f;      // Zasiêg ataku
    public float attackCooldown = 1f;   // Czas pomiêdzy atakami
    public float attackTime = 4f;
    public float patrolDetectionRadius = 10f;  // Odleg³oœæ wykrywania gracza
    public float chaseDetectionRadius = 20f;   // Odleg³oœæ wykrywania w trybie poœcigu
    private float patrol;
    public float chaseSpeed = 5f;        // Prêdkoœæ gonienia gracza
    public float patrolSpeed = 2f;       // Prêdkoœæ patrolowania
    public float patrolTime = 3f;        // Czas patrolowania w jednym kierunku
    public float patrolRange = 5f;       // Maksymalny zakres patrolowania od pozycji startowej

    private Transform player;            // Gracz
    private Vector3 startPosition;       // Pozycja startowa wroga
    private Vector3 patrolTarget;        // Aktualny cel patrolowania
    private float patrolTimer;           // Licznik czasu patrolowania
    private bool isChasing = false;      // Czy wróg goni gracza?
    private bool isAttacking = false;
    private float lastAttackTime = 0f;   // Ostatni czas wykonania ataku
    [SerializeField] private PlayerValues playerHealth;
    [SerializeField] private float distanceToPlayer;

    private Rigidbody rb;                // Rigidbody wroga
    [SerializeField] private Animator animator;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // ZnajdŸ gracza
        startPosition = transform.position;  // Zapisz pozycjê startow¹
        patrolTarget = GetRandomPatrolTarget(); // Ustaw pierwszy cel patrolowania
        rb = GetComponent<Rigidbody>(); // Pobierz Rigidbody
        patrol = patrolDetectionRadius;
    }

    void Update()
    {
        if (currentState != EnemyState.Dead)
        {
            if (player == null)
            {
                Patrol(); // Jeœli gracz nie istnieje, wróg patroluje
                return;
            }

            distanceToPlayer = Vector3.Distance(transform.position, player.position);




            if (!isAttacking)
            {
                if (distanceToPlayer <= patrol && distanceToPlayer > attackRadius)
                {
                    currentState = EnemyState.Chasing;
                }
                else if (distanceToPlayer > patrol)
                {
                    currentState = EnemyState.Patroling;
                }
                else if (distanceToPlayer <= attackRadius)
                {
                    currentState = EnemyState.Attacking;

                }
            }

            if (currentState == EnemyState.Attacking)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    Attack();
                }
            }
            else if (currentState == EnemyState.Chasing)
            {
                patrol = chaseDetectionRadius;
                animator.SetBool("Attacking", false); // Wy³¹cz atak
                animator.SetFloat("Blend", 1f);
                ChasePlayer(); // Wróg goni gracza
            }
            else if (currentState == EnemyState.Patroling)
            {
                animator.SetBool("Attacking", false); // Wy³¹cz atak
                animator.SetFloat("Blend", .5f);
                Patrol(); // Wróg patroluje
            }
        }
    }

    private void Attack()
    {
        // Zatrzymaj ruch wroga
        rb.velocity = Vector3.zero;

        StartCoroutine(AttackTime());


    }

    IEnumerator AttackTime()
    {
        animator.SetBool("Attacking", true); // Animacja ataku

        yield return new WaitForSeconds(attackTime);

        if (distanceToPlayer <= patrol && distanceToPlayer > attackRadius)
        {
            currentState = EnemyState.Chasing;
        }
        else if (distanceToPlayer > patrol)
        {
            currentState = EnemyState.Patroling;
        }
        else if (distanceToPlayer <= attackRadius)
        {
            Debug.Log("Player is in Attack Range");
            playerHealth.RemoveValue(10);
            isAttacking = false;
            animator.SetBool("Attacking", false); // Wy³¹cz atak

        }
        yield break;
    }


    private void ChasePlayer()
    {
        if (currentState != EnemyState.Dead)
        {
            isChasing = true;
            isAttacking = false;

            // Oblicz kierunek do gracza
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Rotacja w kierunku gracza
            RotateTowards(directionToPlayer);

            // Ograniczenie prêdkoœci poruszania
            Vector3 move = Vector3.ClampMagnitude(directionToPlayer * chaseSpeed * Time.deltaTime, chaseSpeed * Time.deltaTime);

            // Porusz siê w stronê gracza
            rb.MovePosition(transform.position + move);
        }
    }

    private void Patrol()
    {
        if (isChasing)
        {
            patrol = patrolDetectionRadius;
            isChasing = false;
            patrolTarget = GetRandomPatrolTarget(); // Po zakoñczeniu gonienia ustaw nowy cel patrolowania
        }

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolTime || Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            patrolTarget = GetRandomPatrolTarget(); // Wybierz nowy cel patrolowania
            patrolTime = Random.Range(2f, 5f); // Losowy czas patrolowania
            patrolTimer = 0f;
        }

        // Oblicz kierunek do celu patrolowania
        Vector3 directionToTarget = (patrolTarget - transform.position).normalized;

        // Rotacja w kierunku patrolowania
        RotateTowards(directionToTarget);

        // Porusz siê w stronê celu patrolowania
        rb.MovePosition(transform.position + directionToTarget * patrolSpeed * Time.deltaTime);
    }

    private Vector3 GetRandomPatrolTarget()
    {
        // Wybierz losowy punkt w zasiêgu patrolowania
        Vector3 randomOffset = new Vector3(
            Random.Range(-patrolRange, patrolRange),
            0f,
            Random.Range(-patrolRange, patrolRange)
        );

        return startPosition + randomOffset;
    }

    private void RotateTowards(Vector3 direction)
    {
        // Jeœli kierunek nie jest zerowy
        if (direction != Vector3.zero)
        {
            // Oblicz rotacjê w kierunku poruszania siê (tylko na osi Y)
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            enemyCount.RemoveValue(1f);
            rb.isKinematic = true;
            animator.SetBool("isDead", true);
            Destroy(gameObject, 2f); // Zniszcz wroga po trafieniu pociskiem
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patrol); // Sfera wykrywania gracza

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, patrolRange); // Sfera patrolowania

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius); // Sfera ataku
    }
}

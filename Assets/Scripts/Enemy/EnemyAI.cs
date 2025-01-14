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
    public float attackRadius = 2f;    
    public float attackCooldown = 1f;
    public float attackTime = 4f;
    public float patrolDetectionRadius = 10f; 
    public float chaseDetectionRadius = 20f; 
    private float _patrol;
    public float chaseSpeed = 5f; 
    public float patrolSpeed = 2f; 
    public float patrolTime = 3f;   
    public float patrolRange = 5f; 

    private Transform _playerTransform;  
    private Vector3 _startPosition; 
    private Vector3 _patrolTarget; 
    private float _patrolTimer; 
    private bool _isChasing = false;  
    private bool _isAttacking = false;
    private float _lastAttackTime = 0f;  
    [SerializeField] private PlayerValues playerHealth;
    [SerializeField] private float distanceToPlayer;

    private Rigidbody rb;  
    [SerializeField] private Animator animator;


    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform; 
        _startPosition = transform.position; 
        _patrolTarget = GetRandomPatrolTarget(); 
        rb = GetComponent<Rigidbody>(); 
        _patrol = patrolDetectionRadius;
    }

    void Update()
    {
        if (currentState != EnemyState.Dead)
        {
            if (_playerTransform == null)
            {
                Patrol(); 
                return;
            }

            distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            if (!_isAttacking)
            {
                if (distanceToPlayer <= _patrol && distanceToPlayer > attackRadius)
                {
                    currentState = EnemyState.Chasing;
                }
                else if (distanceToPlayer > _patrol)
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
                if (!_isAttacking)
                {
                    _isAttacking = true;
                    Attack();
                }
            }
            else if (currentState == EnemyState.Chasing)
            {
                _patrol = chaseDetectionRadius;
                animator.SetBool("Attacking", false); 
                animator.SetFloat("Blend", 1f);
                ChasePlayer(); 
            }
            else if (currentState == EnemyState.Patroling)
            {
                animator.SetBool("Attacking", false); 
                animator.SetFloat("Blend", .5f);
                Patrol(); 
            }
        }
    }

    private void Attack()
    {
        rb.velocity = Vector3.zero;
        StartCoroutine(AttackTime());
    }

    IEnumerator AttackTime()
    {
        animator.SetBool("Attacking", true); 

        yield return new WaitForSeconds(attackTime);

        if (distanceToPlayer <= _patrol && distanceToPlayer > attackRadius)
        {
            currentState = EnemyState.Chasing;
        }
        else if (distanceToPlayer > _patrol)
        {
            currentState = EnemyState.Patroling;
        }
        else if (distanceToPlayer <= attackRadius)
        {
            Debug.Log("Player is in Attack Range");
            playerHealth.RemoveValue(10);
            _isAttacking = false;
            animator.SetBool("Attacking", false); 
        }
        yield break;
    }

    private void ChasePlayer()
    {
        if (currentState != EnemyState.Dead)
        {
            _isChasing = true;
            _isAttacking = false;

            Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;
            RotateTowards(directionToPlayer);
            Vector3 move = Vector3.ClampMagnitude(directionToPlayer * chaseSpeed * Time.deltaTime, chaseSpeed * Time.deltaTime);
            rb.MovePosition(transform.position + move);
        }
    }

    private void Patrol()
    {
        if (_isChasing)
        {
            _patrol = patrolDetectionRadius;
            _isChasing = false;
            _patrolTarget = GetRandomPatrolTarget(); 
        }

        _patrolTimer += Time.deltaTime;

        if (_patrolTimer >= patrolTime || Vector3.Distance(transform.position, _patrolTarget) < 0.5f)
        {
            _patrolTarget = GetRandomPatrolTarget(); 
            patrolTime = Random.Range(2f, 5f);
            _patrolTimer = 0f;
        }

        Vector3 directionToTarget = (_patrolTarget - transform.position).normalized;
        RotateTowards(directionToTarget);
        rb.MovePosition(transform.position + directionToTarget * patrolSpeed * Time.deltaTime);
    }

    private Vector3 GetRandomPatrolTarget()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-patrolRange, patrolRange),
            0f,
            Random.Range(-patrolRange, patrolRange)
        );

        return _startPosition + randomOffset;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
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
            Destroy(gameObject, 2f); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _patrol); 

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_startPosition, patrolRange); 

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

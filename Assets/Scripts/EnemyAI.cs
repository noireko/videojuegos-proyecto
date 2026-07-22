using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 4f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    private int currentHP;
    private float lastAttackTime;
    private Transform player;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && distance > stopDistance)
        {
            Vector2 direction = player.position - transform.position;
            movement = direction.normalized;
        }
        else
        {
            movement = Vector2.zero;
        }

        if (distance <= stopDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);

            lastAttackTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} daño. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
            Destroy(gameObject);
    }
}
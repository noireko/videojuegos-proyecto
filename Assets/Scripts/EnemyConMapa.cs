using UnityEngine;

/// <summary>
/// Reemplaza EnemyAI en el enemigo especial que debe droppear el mapa.
/// Toda la lógica de movimiento y ataque es idéntica a EnemyAI;
/// solo sobreescribe la muerte para instanciar el drop.
/// </summary>
public class EnemyConMapa : MonoBehaviour, IDamageable
{
    [Header("Movimiento / Detección")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 4f;
    [SerializeField] private float stopDistance = 0.5f;

    [Header("Combate")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Drop")]
    [Tooltip("Prefab del mapa que caerá al suelo al morir este enemigo.")]
    [SerializeField] private GameObject mapaDropPrefab;

    [Tooltip("Offset respecto a la posición del enemigo donde aparece el drop.")]
    [SerializeField] private Vector2 dropOffset = Vector2.zero;

    // ── Internos ──────────────────────────────────────────────
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
        if (player == null) return;

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
            Morir();
    }

    private void Morir()
    {
        // Droppear el mapa en la posición del enemigo
        if (mapaDropPrefab != null)
        {
            Vector3 spawnPos = transform.position + (Vector3)dropOffset;
            Instantiate(mapaDropPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: falta asignar mapaDropPrefab.");
        }

        Destroy(gameObject);
    }
}

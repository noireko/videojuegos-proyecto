using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] int damage = 25;
    [SerializeField] float lifetime = 2f;

    Rigidbody2D rb;
    Vector2 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        if (rb != null)
            rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(damage);

        Destroy(gameObject);
    }
}
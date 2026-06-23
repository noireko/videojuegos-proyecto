using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float range = 20f;
    [SerializeField] LayerMask shootableLayer;

    float lastFireTime;
    Camera cam;
    Animator animator;

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isAiming = animator.GetBool("isWeaponReady");

        if (isAiming && Input.GetMouseButton(0) && Time.time >= lastFireTime + fireRate)
        {
            Disparar();
            lastFireTime = Time.time;
        }
    }

    void Disparar()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = (mouseWorld - (Vector2)transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, range, shootableLayer);

        if (hit.collider != null)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(25);

            Debug.Log($"Impacto en: {hit.collider.name}");
        }

        Debug.DrawRay(transform.position, direccion * range, Color.red, 0.1f);
    }
}
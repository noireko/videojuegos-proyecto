using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float range = 20f;
    [SerializeField] LayerMask shootableLayer;

    float lastFireTime;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastFireTime + fireRate)
        {
            Disparar();
            lastFireTime = Time.time;
        }
    }

    void Disparar()
    {
        // Posición del mouse en el mundo
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

        // Dirección desde el jugador hacia el mouse
        Vector2 direccion = (mouseWorld - (Vector2)transform.position).normalized;

        // Raycast en esa dirección
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, range, shootableLayer);

        if (hit.collider != null)
        {
            Debug.Log($"Impacto en: {hit.collider.name}");

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(25);
        }

        // Ver el rayo en el editor
        Debug.DrawRay(transform.position, direccion * range, Color.red, 0.1f);
    }
}
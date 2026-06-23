using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float range = 20f;
    [SerializeField] LayerMask shootableLayer;
    [SerializeField] GameObject bulletPrefab;      // ← nuevo
    [SerializeField] Transform firePoint;          // ← nuevo

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

        // Instanciar bala
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bala = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bala.GetComponent<Bullet>().SetDirection(direccion);
        }

        Debug.DrawRay(transform.position, direccion * range, Color.red, 0.1f);
    }
}
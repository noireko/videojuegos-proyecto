using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] GameObject bulletPrefab;

    [Header("FirePoints")]
    [SerializeField] Transform fp_Right;
    [SerializeField] Transform fp_UpRight;
    [SerializeField] Transform fp_Up;
    [SerializeField] Transform fp_UpLeft;
    [SerializeField] Transform fp_Left;
    [SerializeField] Transform fp_DownLeft;
    [SerializeField] Transform fp_Down;
    [SerializeField] Transform fp_DownRight;

    float lastFireTime;
    Camera cam;
    Animator animator;
    Transform currentFirePoint;

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    private bool hasGun = false;

    void Update()
    {
        if (!hasGun) return; // si no tiene arma, no hace nada

        ActualizarFirePoint();

        bool isAiming = animator.GetBool("isWeaponReady");
        if (isAiming && Input.GetMouseButton(0) && Time.time >= lastFireTime + fireRate)
        {
            Disparar();
            lastFireTime = Time.time;
        }
    }

    void ActualizarFirePoint()
    {
        int aimX = Mathf.RoundToInt(animator.GetFloat("aimX"));
        int aimY = Mathf.RoundToInt(animator.GetFloat("aimY"));

        if (aimX == 1 && aimY == 0) currentFirePoint = fp_Right;
        else if (aimX == 1 && aimY == 1) currentFirePoint = fp_UpRight;
        else if (aimX == 0 && aimY == 1) currentFirePoint = fp_Up;
        else if (aimX == -1 && aimY == 1) currentFirePoint = fp_UpLeft;
        else if (aimX == -1 && aimY == 0) currentFirePoint = fp_Left;
        else if (aimX == -1 && aimY == -1) currentFirePoint = fp_DownLeft;
        else if (aimX == 0 && aimY == -1) currentFirePoint = fp_Down;
        else if (aimX == 1 && aimY == -1) currentFirePoint = fp_DownRight;
    }

    void Disparar()
    {
        if (bulletPrefab == null || currentFirePoint == null) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = (mouseWorld - (Vector2)transform.position).normalized;

        GameObject bala = Instantiate(bulletPrefab, currentFirePoint.position, Quaternion.identity);
        bala.GetComponent<Bullet>().SetDirection(direccion);
    }

    public void PickupGun()
    {
        hasGun = true;
        Inventory.instance.AddItem("Pistola", 1);
        Debug.Log("Pistola recogida");

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.EnableGun();
    }
}
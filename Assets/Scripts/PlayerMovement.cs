using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float idleDelay = 0.1f;

    private float currentSpeed;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    private int lastX = 0;
    private int lastY = -1;

    // ✅ Variables separadas para la dirección de apuntado
    private int lastAimX = -999;
    private int lastAimY = -999;

    private float idleTimer = 0f;
    private bool isLocked = false;

    void Start()
    {
        Debug.Log("PlayerMovement iniciado en: " + gameObject.name);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentSpeed = walkSpeed;

        animator.SetInteger("moveX", lastX);
        animator.SetInteger("moveY", lastY);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAiming", false);
        animator.SetBool("isWeaponReady", false);
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;

        if (locked)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;

            animator.SetBool("isMoving", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAiming", false);
            animator.SetBool("isWeaponReady", false);
        }
    }

    void Update()
    {
        if (isLocked)
            return;

        bool isAiming = Input.GetMouseButton(1);
        bool wasAiming = animator.GetBool("isAiming");

        animator.SetBool("isAiming", isAiming);

        if (!isAiming && wasAiming)
        {
            animator.SetBool("isWeaponReady", false);
            }

        if (isAiming)
        {

            movement = Vector2.zero;

            animator.SetBool("isMoving", false);
            animator.SetBool("isRunning", false);

            // Al empezar a apuntar, resetea el cache para forzar el primer seteo
            if (!wasAiming)
            {
                lastAimX = -999;
                lastAimY = -999;
                SetDirectionToMouse();
            }

            // Mientras el arma está lista, actualiza dirección con el mouse
            if (animator.GetBool("isWeaponReady"))
            {
                SetDirectionToMouse();
            }

            return;
        }

        // Al dejar de apuntar, resetea el cache de aim
        if (wasAiming)
        {
            lastAimX = -999;
            lastAimY = -999;
        }

        // --- Movimiento normal (sin apuntar) ---

        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y).normalized;

        bool isMoving = x != 0 || y != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;

        currentSpeed = isRunning ? runSpeed : walkSpeed;

        animator.SetBool("isRunning", isRunning);

        if (isMoving)
        {
            if (x != lastX || y != lastY)
            {
                lastX = x;
                lastY = y;

                animator.SetInteger("moveX", lastX);
                animator.SetInteger("moveY", lastY);
            }

            animator.SetBool("isMoving", true);
            idleTimer = idleDelay;
        }
        else
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
            }
        }
    }

    void SetDirectionToMouse()
    {
        Debug.Log("SetDirectionToMouse llamado");
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorldPos - transform.position;

        if (dir.magnitude < 0.1f) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        int aimX = 0, aimY = 0;

        if      (angle >= -22.5f  && angle <   22.5f) { aimX =  1; aimY =  0; }
        else if (angle >=  22.5f  && angle <   67.5f) { aimX =  1; aimY =  1; }
        else if (angle >=  67.5f  && angle <  112.5f) { aimX =  0; aimY =  1; }
        else if (angle >= 112.5f  && angle <  157.5f) { aimX = -1; aimY =  1; }
        else if (angle >=  157.5f || angle  < -157.5f) { aimX = -1; aimY =  0; }
        else if (angle >= -157.5f && angle < -112.5f) { aimX = -1; aimY = -1; }
        else if (angle >= -112.5f && angle <  -67.5f) { aimX =  0; aimY = -1; }
        else if (angle >=  -67.5f && angle <  -22.5f) { aimX =  1; aimY = -1; }

        // Guard: usa lastAimX/Y separados del movimiento
        if (aimX == lastAimX && aimY == lastAimY)
{
    Debug.Log("Guard bloqueó - aimX: " + aimX + " aimY: " + aimY);
    return;
}

Debug.Log("Seteando aimX: " + aimX + " aimY: " + aimY);
animator.SetFloat("aimX", (float)aimX);
animator.SetFloat("aimY", (float)aimY);

        lastAimX = aimX;
        lastAimY = aimY;

        animator.SetFloat("aimX", (float)aimX);
        animator.SetFloat("aimY", (float)aimY);
    }

    public void WeaponReady()
    {
        animator.SetBool("isWeaponReady", true);
    }

    public void WeaponHolstered()
    {
        animator.SetBool("isWeaponReady", false);
    }

    void FixedUpdate()
    {
        if (isLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.MovePosition(
            rb.position + movement * currentSpeed * Time.fixedDeltaTime
        );
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder =
            Mathf.RoundToInt(transform.position.y * -100);
    }
}

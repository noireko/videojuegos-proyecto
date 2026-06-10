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

    private float idleTimer = 0f;
    private bool isLocked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentSpeed = walkSpeed;

        animator.SetInteger("moveX", lastX);
        animator.SetInteger("moveY", lastY);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAiming", false);
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
        }
    }

    void Update()
    {
        if (isLocked)
            return;

        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y).normalized;

        bool isMoving = x != 0 || y != 0;
        bool isAiming = Input.GetMouseButton(1);
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving && !isAiming;

        currentSpeed = isRunning ? runSpeed : walkSpeed;

        animator.SetBool("isAiming", isAiming);
        animator.SetBool("isRunning", isRunning);

        if (isAiming)
{
    SetDirectionToMouse();
    animator.SetBool("isMoving", false);
    animator.SetBool("isRunning", false);
}

        if (isMoving)
        {
            lastX = x;
            lastY = y;

            animator.SetInteger("moveX", lastX);
            animator.SetInteger("moveY", lastY);
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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorldPos - transform.position;

        if (dir.magnitude < 0.1f)
            return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        int aimX = 0;
        int aimY = 0;

        if (angle >= -22.5f && angle < 22.5f)
        {
            aimX = 1; aimY = 0;
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            aimX = 1; aimY = 1;
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            aimX = 0; aimY = 1;
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            aimX = -1; aimY = 1;
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            aimX = -1; aimY = 0;
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            aimX = -1; aimY = -1;
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            aimX = 0; aimY = -1;
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            aimX = 1; aimY = -1;
        }

        lastX = aimX;
        lastY = aimY;

        animator.SetInteger("moveX", lastX);
        animator.SetInteger("moveY", lastY);
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

    public void WeaponHolstered()
{
    animator.SetBool("isWeaponReady", false);
}

public void WeaponReady()
{
    animator.SetBool("isWeaponReady", true);
}
}
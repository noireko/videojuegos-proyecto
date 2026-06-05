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
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;

        if (locked)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);
            animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
        }
    }

    void Update()
    {
        if (isLocked)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y).normalized;

        bool isMoving = x != 0 || y != 0;

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
            }
        }
    }

    void FixedUpdate()
    {
        if (isLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.MovePosition(
            rb.position +
            movement * currentSpeed * Time.fixedDeltaTime
        );
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder =
            Mathf.RoundToInt(transform.position.y * -100);
    }
}
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    private int lastX = 0;
    private int lastY = -1;

    private float idleDelay = 0.1f;
    private float idleTimer = 0f;

    private int prevX = 0;
    private int prevY = 0;

    private bool isLocked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        }
    }

    void Update()
    {
        if (isLocked)
            return;

        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y).normalized;

        if (x != 0 || y != 0)
        {
            bool isFullDiagonal = x != 0 && y != 0;
            bool comingFromIdle = idleTimer <= 0f;

            int prevComponents = (prevX != 0 ? 1 : 0) + (prevY != 0 ? 1 : 0);
            int currComponents = (x != 0 ? 1 : 0) + (y != 0 ? 1 : 0);
            bool addedKey = currComponents > prevComponents;

            bool completeDirectionChange = (prevX != 0 || prevY != 0) &&
                                           (x != prevX && y != prevY);

            if (isFullDiagonal || comingFromIdle || addedKey || completeDirectionChange)
            {
                lastX = x;
                lastY = y;

                animator.SetInteger("moveX", lastX);
                animator.SetInteger("moveY", lastY);
            }

            idleTimer = idleDelay;
            animator.SetBool("isMoving", true);
        }
        else
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                animator.SetBool("isMoving", false);
            }
        }

        prevX = x;
        prevY = y;
    }

    void FixedUpdate()
    {
        if (isLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }
}
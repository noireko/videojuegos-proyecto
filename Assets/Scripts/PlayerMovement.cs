using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    private int lastX = 0;
    private int lastY = -1;

    private float idleDelay = 0.1f;
    private float idleTimer = 0f;

    private int prevX = 0;
    private int prevY = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetInteger("moveX", lastX);
        animator.SetInteger("moveY", lastY);
    }

    void Update()
    {
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
        rb.linearVelocity = movement * speed;
    }
}
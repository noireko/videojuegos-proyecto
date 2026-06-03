using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    private int lastX = 0;
    private int lastY = -1;

   [SerializeField] private float idleDelay = 0.15f;
[SerializeField] private float idleTimer = 0f;

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
        if (x != lastX || y != lastY)
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
}

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }
}
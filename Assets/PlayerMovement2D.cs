using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");

        movement = new Vector2(x, y).normalized;

        bool isMoving = movement != Vector2.zero;

        animator.SetBool("isMoving", isMoving);
        animator.SetInteger("moveX", x);
        animator.SetInteger("moveY", y);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }
}
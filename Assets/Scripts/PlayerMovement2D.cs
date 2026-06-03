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

        animator.SetBool("isMoving", movement != Vector2.zero);
        animator.SetInteger("moveX", x);
        animator.SetInteger("moveY", y);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }
}

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRadius = 1.5f;
    [SerializeField] LayerMask interactableLayer;

    IInteractable currentTarget;

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position, interactRadius, interactableLayer);

        currentTarget = hit != null ? hit.GetComponent<IInteractable>() : null;

        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
            currentTarget.Interact();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
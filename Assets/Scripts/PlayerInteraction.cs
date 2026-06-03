using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float hitInterval = 0.5f;

    private IInteractable currentTarget;
    private PlayerMovement playerMovement;
    private Animator animator;

    private float hitTimer = 0f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            interactRadius,
            interactableLayer
        );

        currentTarget = hit != null ? hit.GetComponent<IInteractable>() : null;

        if (hitTimer > 0f)
            hitTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.E) && currentTarget != null)
        {
            playerMovement.SetLocked(true);
            animator.SetBool("isChopping", true);

            if (hitTimer <= 0f)
            {
                currentTarget.Interact();
                hitTimer = hitInterval;
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            playerMovement.SetLocked(false);
            animator.SetBool("isChopping", false);
            hitTimer = 0f;
        }

        if (currentTarget == null && Input.GetKey(KeyCode.E))
        {
            playerMovement.SetLocked(false);
            animator.SetBool("isChopping", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
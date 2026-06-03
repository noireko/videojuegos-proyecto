using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRadius = 1.5f;
    [SerializeField] LayerMask interactableLayer;

    [SerializeField] float hitInterval = 0.5f;

    IInteractable currentTarget;
    PlayerMovement playerMovement;

    private float hitTimer = 0f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            interactRadius,
            interactableLayer);

        currentTarget = hit != null ? hit.GetComponent<IInteractable>() : null;

        if (hitTimer > 0)
            hitTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.E) && currentTarget != null)
        {
            playerMovement.SetLocked(true);

            if (hitTimer <= 0)
            {
                currentTarget.Interact();
                hitTimer = hitInterval;
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            playerMovement.SetLocked(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
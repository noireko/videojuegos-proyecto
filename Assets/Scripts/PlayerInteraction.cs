using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRadius = 1.5f;
    [SerializeField] LayerMask interactableLayer;

    IInteractable currentTarget;
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position, interactRadius, interactableLayer);

        currentTarget = hit != null ? hit.GetComponent<IInteractable>() : null;

        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            playerMovement.SetLocked(true);
            currentTarget.Interact();
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
using UnityEngine;

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
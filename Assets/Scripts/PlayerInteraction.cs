using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRadius = 1.5f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] GameObject interactIcon; // ícono de E

    IInteractable currentTarget;
    GameObject currentIconInstance;

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position, interactRadius, interactableLayer);

        if (hit != null)
        {
            currentTarget = hit.GetComponent<IInteractable>();

            // Mostrar ícono sobre el objeto
            if (currentIconInstance == null)
                currentIconInstance = Instantiate(interactIcon, 
                    hit.transform.position + Vector3.up * 1f, 
                    Quaternion.identity);
            else
                currentIconInstance.transform.position = 
                    hit.transform.position + Vector3.up * 1f;
        }
        else
        {
            currentTarget = null;

            // Ocultar ícono
            if (currentIconInstance != null)
            {
                Destroy(currentIconInstance);
                currentIconInstance = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
            currentTarget.Interact();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
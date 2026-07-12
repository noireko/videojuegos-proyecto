using UnityEngine;

public class AdrenalineVialPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private int healAmount = 40;
    [SerializeField] private float regenDuration = 3f;
    [SerializeField] private GameObject floatingTextPrefab; // opcional

    private bool usado = false;

    // Requerido por la interfaz IInteractable: este objeto no usa la animación de "cortar"
    public bool UsesChopAnimation => false;

    // Este método lo llama automáticamente PlayerInteraction.cs cuando el jugador
    // está cerca Y aprieta la tecla E (ver PlayerInteraction.cs línea ~62)
    public void Interact()
    {
        if (usado) return; // evita que se use dos veces por accidente
        usado = true;

        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.StartFastRegen(healAmount, regenDuration);
        }

        if (floatingTextPrefab != null)
        {
            GameObject textGO = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            FloatingText ft = textGO.GetComponent<FloatingText>();
            if (ft != null) ft.SetText("+" + healAmount + " HP");
        }

        Destroy(gameObject);
    }
}
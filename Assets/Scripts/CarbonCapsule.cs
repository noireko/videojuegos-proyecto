using UnityEngine;

// Pickup de recarga de oxígeno. Sigue el mismo patrón que ItemPickup.cs
// (trigger 2D + tag "Player" + Destroy), ya que en el GDD la cápsula se
// recolecta automáticamente al tocarla, sin necesidad de apretar "E".
// El flote se agrega en el editor con el componente FloatingItemEffect.cs
// ya existente, en vez de duplicar esa lógica acá.
public class CarbonCapsule : MonoBehaviour
{
    [Header("Recarga de Oxígeno")]
    [SerializeField] private float cantidadOxigeno = 20f;

    [Header("Feedback")]
    [SerializeField] private AudioClip sonidoRecoleccion;
    [SerializeField] private GameObject prefabParticulas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        OxygenSystem oxygenSystem = other.GetComponent<OxygenSystem>();
        if (oxygenSystem != null)
            oxygenSystem.AddOxygen(cantidadOxigeno);

        if (sonidoRecoleccion != null)
            AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position);

        if (prefabParticulas != null)
            Instantiate(prefabParticulas, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}

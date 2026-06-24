using UnityEngine;

public class GunPickup : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject gunIcon;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] Transform player; // arrastrá al jugador en el Inspector

    private bool picked = false;

    public bool UsesChopAnimation => false;

    public void Interact()
    {
        if (picked) return;
        picked = true;

        PlayerGun gun = FindFirstObjectByType<PlayerGun>();
        if (gun != null)
            gun.PickupGun();

        if (gunIcon != null)
            gunIcon.SetActive(false);

        if (floatingTextPrefab != null && player != null)
        {
            GameObject textObj = Instantiate(
                floatingTextPrefab,
                player.position + Vector3.up * 1.2f,
                Quaternion.identity
            );
            textObj.GetComponent<FloatingText>().SetText("Está algo oxidada, pero puede servirme");
        }
    }
}
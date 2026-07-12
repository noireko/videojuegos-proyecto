using UnityEngine;

public class GunPickup : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject gunIcon;
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

        if (InteractionTextUI.instance != null)
            InteractionTextUI.instance.Show();
    }
}
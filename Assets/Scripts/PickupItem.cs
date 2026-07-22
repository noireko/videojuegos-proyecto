using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.instance.AddItem(itemName, amount);
            Destroy(gameObject);
        }
    }
}
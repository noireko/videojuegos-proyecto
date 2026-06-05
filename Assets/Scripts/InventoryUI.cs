using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
        }
    }
}
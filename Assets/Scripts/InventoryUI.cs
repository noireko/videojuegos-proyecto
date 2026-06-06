using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemUI
    {
        public string itemName;
        public Image icon;
        public TMP_Text amountText;
    }

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private ItemUI[] itemUIs;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        foreach (ItemUI itemUI in itemUIs)
        {
            int amount = 0;

            if (Inventory.instance.Items.ContainsKey(itemUI.itemName))
                amount = Inventory.instance.Items[itemUI.itemName];

            itemUI.amountText.text = amount.ToString();

            bool hasItem = amount > 0;
            itemUI.icon.enabled = hasItem;
            itemUI.amountText.enabled = hasItem;
        }
    }
}
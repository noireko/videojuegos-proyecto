using UnityEngine;

public class InventoryWindowUI : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject constructionPanel;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        constructionPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWindow();
        }
    }

    public void ToggleWindow()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            ShowInventory();
        }
        else
        {
            inventoryPanel.SetActive(false);
            constructionPanel.SetActive(false);
        }
    }

    public void ShowInventory()
    {
        isOpen = true;

        inventoryPanel.SetActive(true);
        constructionPanel.SetActive(false);
    }

    public void ShowConstruction()
    {
        isOpen = true;

        inventoryPanel.SetActive(false);
        constructionPanel.SetActive(true);
    }

    public void ToggleInventoryConstruction()
    {
        isOpen = true;

        bool showingInventory = inventoryPanel.activeSelf;

        inventoryPanel.SetActive(!showingInventory);
        constructionPanel.SetActive(showingInventory);
    }

    public void CloseWindow()
    {
        isOpen = false;

        inventoryPanel.SetActive(false);
        constructionPanel.SetActive(false);
    }
}
using UnityEngine;

public class InventoryWindowUI : MonoBehaviour
{
    [Header("Ventana completa")]
    [SerializeField] private GameObject inventoryWindow;

    [Header("Paneles")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject constructionPanel;

    private bool isOpen = false;

    void Start()
    {
        inventoryWindow.SetActive(false);

        inventoryPanel.SetActive(true);
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
        inventoryWindow.SetActive(isOpen);

        if (isOpen)
        {
            ShowInventory();
        }
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        constructionPanel.SetActive(false);
    }

    public void ShowConstruction()
    {
        inventoryPanel.SetActive(false);
        constructionPanel.SetActive(true);
    }

    public void ToggleInventoryConstruction()
    {
        bool showingInventory = inventoryPanel.activeSelf;

        inventoryPanel.SetActive(!showingInventory);
        constructionPanel.SetActive(showingInventory);
    }

    public void CloseWindow()
    {
        isOpen = false;
        inventoryWindow.SetActive(false);
    }
}
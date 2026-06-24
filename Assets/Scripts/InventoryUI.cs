using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] Transform gridObjetos;
    [SerializeField] Transform gridArmas;
    [SerializeField] GameObject slotPrefab;

    [Header("Items definidos")]
    [SerializeField] List<ItemDefinition> itemsDefinidos;

    Dictionary<string, SlotUI> slots = new Dictionary<string, SlotUI>();

    void Start()
    {
        inventoryPanel.SetActive(false);
        InicializarSlots();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool abierto = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!abierto);

            if (!abierto)
                RefrescarUI();
        }
    }

    void InicializarSlots()
    {
        foreach (var item in itemsDefinidos)
        {
            Transform grid = item.esArma ? gridArmas : gridObjetos;
            GameObject slotGO = Instantiate(slotPrefab, grid);

            // Forzar escala 1 en todos los hijos
            foreach (Transform child in slotGO.GetComponentsInChildren<Transform>())
                if (child.GetComponent<Canvas>() != null)
                    child.localScale = Vector3.one;

            SlotUI slot = new SlotUI
            {
                icon = slotGO.GetComponentInChildren<Image>(),
                amountText = slotGO.GetComponentInChildren<TextMeshProUGUI>(),
                itemName = item.nombre
            };

            slot.icon.sprite = item.icono;
            slot.icon.color = new Color(1, 1, 1, 0f);
            slot.amountText.text = "";
            slots[item.nombre] = slot;
        }
    }

    public void RefrescarUI()
    {
        foreach (var kvp in slots)
        {
            int cantidad = Inventory.instance.GetItemAmount(kvp.Key);
            SlotUI slot = kvp.Value;

            Debug.Log($"Refrescando {kvp.Key}: cantidad={cantidad}, amountText null={slot.amountText == null}");

            if (cantidad > 0)
            {
                slot.icon.color = Color.white;
                slot.amountText.text = cantidad.ToString();
            }
            else
            {
                slot.icon.color = new Color(1, 1, 1, 0f);
                slot.amountText.text = "";
            }
        }
    }
}

[System.Serializable]
public class ItemDefinition
{
    public string nombre;
    public Sprite icono;
    public bool esArma;
}

class SlotUI
{
    public string itemName;
    public Image icon;
    public TextMeshProUGUI amountText;
}
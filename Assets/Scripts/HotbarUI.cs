using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    [Header("Slots fijos (arrastrá los 8 GameObjects de tu hotbar en orden)")]
    [SerializeField] private List<HotbarSlot> slots;

    void Start()
    {
        RefrescarUI();
    }

    void Update()
    {
        RefrescarUI();
    }

    public void RefrescarUI()
    {
        foreach (var slot in slots)
        {
            if (string.IsNullOrEmpty(slot.itemName))
            {
                if (slot.icon != null)
                    slot.icon.color = new Color(1, 1, 1, 0f);
                if (slot.amountText != null)
                    slot.amountText.text = "";
                continue;
            }

            int cantidad = Inventory.instance.GetItemAmount(slot.itemName);

            if (cantidad > 0)
            {
                slot.icon.sprite = slot.iconoDefault;
                slot.icon.color = Color.white;
                slot.amountText.text = cantidad >= 2 ? cantidad.ToString() : "";
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
public class HotbarSlot
{
    public string itemName;
    public Image icon;
    public Sprite iconoDefault;
    public TextMeshProUGUI amountText;
}
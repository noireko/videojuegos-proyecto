using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingSlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] Button buildButton;
    [SerializeField] Image buildButtonImage;

    [Header("Sprites del botón")]
    [SerializeField] Sprite botonVerde;
    [SerializeField] Sprite botonGris;

    [Header("Costo")]
    [SerializeField] string itemName = "Madera";
    [SerializeField] int amount = 1;

    void Update()
    {
        bool tieneMateriales = Inventory.instance.HasItem(itemName, amount);

        amountText.color = tieneMateriales ? Color.green : Color.red;
        buildButtonImage.sprite = tieneMateriales ? botonVerde : botonGris;
        buildButton.interactable = tieneMateriales;
    }
}
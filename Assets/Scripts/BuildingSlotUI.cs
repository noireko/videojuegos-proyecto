using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingSlotUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] Button buildButton;
    [SerializeField] Image buildButtonImage;

    [Header("Colores")]
    [SerializeField] Color colorSuficiente = Color.green;
    [SerializeField] Color colorInsuficiente = Color.red;
    [SerializeField] Color botonActivo;
    [SerializeField] Color botonInactivo;

    [Header("Costo")]
    [SerializeField] string itemName = "Madera";
    [SerializeField] int amount = 1;

    void Update()
    {
        bool tieneMateriales = Inventory.instance.HasItem(itemName, amount);

        // Cambiar color del texto
        amountText.color = tieneMateriales ? colorSuficiente : colorInsuficiente;

        // Cambiar color del botón
        buildButtonImage.color = tieneMateriales ? botonActivo : botonInactivo;

        // Activar o desactivar el botón
        buildButton.interactable = tieneMateriales;
    }
}
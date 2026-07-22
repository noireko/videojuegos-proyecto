using UnityEngine;
using UnityEngine.UI;

public class OxygenBarUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private OxygenSystem oxygenSystem;
    [SerializeField] private Image barraOxigeno; // Image con Image Type = Filled

    [Header("Umbrales de Color (según GDD)")]
    [SerializeField] private Color colorOptimo = new Color(0.3f, 0.7f, 1f);       // 70-100%
    [SerializeField] private Color colorAdvertencia = new Color(1f, 0.85f, 0.2f); // 30-69%
    [SerializeField] private Color colorCritico = new Color(0.9f, 0.15f, 0.15f);  // 0-29%

    void OnEnable()
    {
        if (oxygenSystem != null)
            oxygenSystem.OnOxygenChanged.AddListener(ActualizarBarra);
    }

    void OnDisable()
    {
        if (oxygenSystem != null)
            oxygenSystem.OnOxygenChanged.RemoveListener(ActualizarBarra);
    }

    void Start()
    {
        if (oxygenSystem != null)
            ActualizarBarra(oxygenSystem.Porcentaje);
    }

    private void ActualizarBarra(float porcentaje)
    {
        if (barraOxigeno == null) return;

        barraOxigeno.fillAmount = porcentaje;

        if (porcentaje >= 0.7f)
            barraOxigeno.color = colorOptimo;
        else if (porcentaje >= 0.3f)
            barraOxigeno.color = colorAdvertencia;
        else
            barraOxigeno.color = colorCritico;
    }
}

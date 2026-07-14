using UnityEngine;

public class CicloDiaNocheUI : MonoBehaviour
{
    [SerializeField] private CicloDiaNoche cicloDiaNoche;
    [SerializeField] private RectTransform indicador;
    [SerializeField] private float posicionXMinima = -100f;
    [SerializeField] private float posicionXMaxima = 100f;

    void Update()
    {
        if (cicloDiaNoche == null || indicador == null) return;

        float x = Mathf.Lerp(posicionXMinima, posicionXMaxima, cicloDiaNoche.ValorCiclo);
        Vector2 pos = indicador.anchoredPosition;
        pos.x = x;
        indicador.anchoredPosition = pos;
    }
}
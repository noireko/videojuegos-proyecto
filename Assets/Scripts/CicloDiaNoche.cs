using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CicloDiaNoche : MonoBehaviour
{
    [SerializeField] private Light2D luzGlobal;
    [SerializeField] private float duracionCicloSegundos = 120f;
    [SerializeField] private float intensidadMaxima = 1f;
    [SerializeField] private float intensidadMinima = 0.15f;

    private float tiempoTranscurrido = 0f;

    public float ValorCiclo { get; private set; } // 0 = noche total, 1 = día total

    void Update()
    {
        if (luzGlobal == null) return;

        tiempoTranscurrido += Time.deltaTime;

        float progreso = (tiempoTranscurrido % duracionCicloSegundos) / duracionCicloSegundos;
        float ciclo = (Mathf.Cos(progreso * Mathf.PI * 2f) + 1f) / 2f;

        luzGlobal.intensity = Mathf.Lerp(intensidadMinima, intensidadMaxima, ciclo);
        ValorCiclo = ciclo;
    }

    public bool EsDeNoche()
    {
        return luzGlobal != null && luzGlobal.intensity < (intensidadMinima + intensidadMaxima) / 2f;
    }
}
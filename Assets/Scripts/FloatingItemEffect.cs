using UnityEngine;

public class FloatingItemEffect : MonoBehaviour
{
    [SerializeField] private float amplitud = 0.1f; // qué tan alto sube y baja (en unidades)
    [SerializeField] private float velocidad = 2f;   // qué tan rápido oscila

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * velocidad) * amplitud;
        transform.position = posicionInicial + new Vector3(0f, offsetY, 0f);
    }
}
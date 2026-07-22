using UnityEngine;
using UnityEngine.Events;

// Evento serializable para poder engancharlo desde el Inspector (barra de HUD, sonidos, etc.)
[System.Serializable]
public class OxygenChangedEvent : UnityEvent<float> { }

public class OxygenSystem : MonoBehaviour
{
    [Header("Configuración de Oxígeno (según GDD)")]
    [SerializeField] private float oxigenoMaximo = 100f;
    [SerializeField] private float consumoPorSegundo = 1f;
    [SerializeField] private float multiplicadorSprint = 1.2f; // correr acelera el consumo un 20%

    [Header("Eventos")]
    public OxygenChangedEvent OnOxygenChanged; // porcentaje actual, de 0 a 1
    public UnityEvent OnOxygenDepleted;        // se dispara una sola vez al llegar a 0 (game over por asfixia)

    private float oxigenoActual;
    private bool corriendo = false;
    private bool agotado = false;

    public float OxigenoActual => oxigenoActual;
    public float OxigenoMaximo => oxigenoMaximo;
    public float Porcentaje => oxigenoMaximo > 0f ? oxigenoActual / oxigenoMaximo : 0f;

    void Start()
    {
        oxigenoActual = oxigenoMaximo;
        NotificarCambio();
    }

    void Update()
    {
        if (agotado) return;

        float consumo = consumoPorSegundo * (corriendo ? multiplicadorSprint : 1f);
        LoseOxygen(consumo * Time.deltaTime);
    }

    // Llamado desde PlayerMovement cuando el jugador está corriendo (Shift)
    public void SetSprinting(bool sprinting)
    {
        corriendo = sprinting;
    }

    // Recarga instantánea, usado por las Cápsulas de Carbón
    public void AddOxygen(float cantidad)
    {
        if (cantidad <= 0f) return;

        oxigenoActual = Mathf.Clamp(oxigenoActual + cantidad, 0f, oxigenoMaximo);
        NotificarCambio();
    }

    public void LoseOxygen(float cantidad)
    {
        if (agotado || cantidad <= 0f) return;

        oxigenoActual = Mathf.Clamp(oxigenoActual - cantidad, 0f, oxigenoMaximo);
        NotificarCambio();

        if (oxigenoActual <= 0f)
        {
            agotado = true;
            OnOxygenDepleted?.Invoke();
        }
    }

    public void RefillFull()
    {
        oxigenoActual = oxigenoMaximo;
        agotado = false;
        NotificarCambio();
    }

    private void NotificarCambio()
    {
        OnOxygenChanged?.Invoke(Porcentaje);
    }
}

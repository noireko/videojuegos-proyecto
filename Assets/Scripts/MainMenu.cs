using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject opcionesPanel;

    void Start()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(false);
    }

    public void NuevaPartida()
    {
        SceneManager.LoadScene(1);
    }

    public void Continuar()
    {
        // Deshabilitado para la demo
    }

    public void Opciones()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(true);
    }

    public void CerrarOpciones()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo...");
    }
}
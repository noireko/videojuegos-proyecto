using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panelGameOver;

    private bool mostrado = false;

    void Start()
    {
        if (panelGameOver != null)
            panelGameOver.SetActive(false);
    }

    // Enganchado desde OxygenSystem.OnOxygenDepleted en el Inspector
    public void MostrarGameOver()
    {
        if (mostrado) return;
        mostrado = true;

        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.SetLocked(true);

        if (panelGameOver != null)
            panelGameOver.SetActive(true);
    }

    // Enganchado desde el botón "Reintentar" en el Inspector
    public void Reintentar()
    {
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.buildIndex);
    }
}

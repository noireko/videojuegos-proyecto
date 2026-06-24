using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicaManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += TerminoVideo;
        videoPlayer.Play();
    }

    void Update()
    {
        // Saltar cinemática con cualquier tecla
        if (Input.anyKeyDown)
            CargarJuego();
    }

    void TerminoVideo(VideoPlayer vp)
    {
        CargarJuego();
    }

    void CargarJuego()
    {
        SceneManager.LoadScene(2);
    }
}
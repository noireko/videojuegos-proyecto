using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicaManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videos;
    [SerializeField] int escenaJuego = 2;

    int currentIndex = 0;

    void Start()
    {
        videoPlayer.loopPointReached += TerminoVideo;
        ReproducirActual();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            CargarJuego();
    }

    void ReproducirActual()
    {
        if (currentIndex >= videos.Length)
        {
            CargarJuego();
            return;
        }

        videoPlayer.clip = videos[currentIndex];
        videoPlayer.Play();
    }

    void TerminoVideo(VideoPlayer vp)
    {
        currentIndex++;
        ReproducirActual();
    }

    void CargarJuego()
    {
        videoPlayer.loopPointReached -= TerminoVideo;
        SceneManager.LoadScene(escenaJuego);
    }
}
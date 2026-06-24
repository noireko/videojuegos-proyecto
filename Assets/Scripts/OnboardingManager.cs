using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] Image slideImage;
    [SerializeField] List<Sprite> slides;
    [SerializeField] int escenaJuego = 3;

    int currentIndex = 0;

    void Start()
    {
        MostrarSlide(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentIndex++;
            MostrarSlide(currentIndex);
        }
    }

    void MostrarSlide(int index)
    {
        if (index >= slides.Count)
        {
            CargarJuego();
            return;
        }

        slideImage.sprite = slides[index];
    }

    void CargarJuego()
    {
        SceneManager.LoadScene(escenaJuego);
    }
}
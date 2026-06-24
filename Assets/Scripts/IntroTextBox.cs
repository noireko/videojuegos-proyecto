using UnityEngine;

public class IntroTextBox : MonoBehaviour
{
    [SerializeField] private GameObject textBoxImage;

    void Start()
    {
        textBoxImage.SetActive(true);
    }

    void Update()
    {
        if (textBoxImage.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            textBoxImage.SetActive(false);
        }
    }
}
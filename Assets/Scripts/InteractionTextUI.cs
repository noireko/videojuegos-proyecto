using UnityEngine;

public class InteractionTextUI : MonoBehaviour
{
    public static InteractionTextUI instance;

    [SerializeField] private GameObject imageObject; // el GameObject con el sprite de texto

    void Awake()
    {
        instance = this;
        imageObject.SetActive(false);
    }

    public void Show()
    {
        imageObject.SetActive(true);
    }

    public void Hide()
    {
        imageObject.SetActive(false);
    }
}
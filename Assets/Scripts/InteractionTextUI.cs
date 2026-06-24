using UnityEngine;

public class InteractionTextUI : MonoBehaviour
{
    public static InteractionTextUI instance;

    [Tooltip("Todos los posibles mensajes/imágenes. El índice 0 es el que ya tenías.")]
    [SerializeField] private GameObject[] imageObjects;

    [SerializeField] private float lifeTime = 2f;
    public float LifeTime => lifeTime;

    private float timer;
    private int currentIndex = -1;

    void Awake()
    {
        instance = this;
        HideAll();
    }

    void Update()
    {
        if (currentIndex < 0) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
            Hide();
    }

    /// <summary>Muestra el imagen en el índice indicado.</summary>
    public void Show(int index = 0)
    {
        HideAll();

        if (index < 0 || index >= imageObjects.Length)
        {
            Debug.LogWarning($"InteractionTextUI: índice {index} fuera de rango.");
            return;
        }

        currentIndex = index;
        imageObjects[currentIndex].SetActive(true);
        timer = lifeTime;
    }

    public void Hide()
    {
        if (currentIndex >= 0 && currentIndex < imageObjects.Length)
            imageObjects[currentIndex].SetActive(false);
        currentIndex = -1;
    }

    private void HideAll()
    {
        foreach (var obj in imageObjects)
            if (obj != null) obj.SetActive(false);
        currentIndex = -1;
    }
}

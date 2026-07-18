using UnityEngine;
using UnityEngine.Video;

public class RadioDeEryx : MonoBehaviour, IInteractable
{
    public bool UsesChopAnimation => cinematicaYaVista;

    [Header("Cinemática")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoPanelUI;
    [SerializeField] private CanvasGroup videoPanelCanvasGroup;

    [Header("Sprites de daño")]
    [SerializeField] private Sprite[] damageSprites;

    [Header("Drop")]
    [SerializeField] private GameObject componentePrefab;
    [SerializeField] private Vector3 dropOffset = new Vector3(0.2f, -0.3f, 0f);

    [Header("Timing")]
    [SerializeField] private float delayAntesDeDestruir = 0.4f;

    private int currentHP = 3;
    private bool cinematicaYaVista = false;
    private bool isDestruyendo = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (videoPanelUI != null)
            videoPanelUI.SetActive(false);
    }

    public string GetPrompt()
    {
        return cinematicaYaVista ? "Destruir radio" : "Interactuar";
    }

    public void Interact()
    {
        if (isDestruyendo) return;

        if (!cinematicaYaVista)
        {
            PlayCinematica();
            return;
        }

        currentHP--;
        if (currentHP <= 0)
        {
            isDestruyendo = true;
            StartCoroutine(DestruirConDelay(delayAntesDeDestruir));
            return;
        }
        ActualizarSprite();
    }

    private void PlayCinematica()
    {
        if (videoPlayer != null)
        {
            if (videoPanelUI != null)
                videoPanelUI.SetActive(true);

            if (videoPanelCanvasGroup != null)
                videoPanelCanvasGroup.alpha = 0f;

            videoPlayer.prepareCompleted += OnVideoPreparado;
            videoPlayer.Prepare();
        }
        else
        {
            if (videoPanelUI != null)
                videoPanelUI.SetActive(true);
            OnCinematicaTerminada(null);
        }
    }

    private void OnVideoPreparado(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnVideoPreparado;

        if (videoPanelCanvasGroup != null)
            videoPanelCanvasGroup.alpha = 1f;

        videoPlayer.Play();
        videoPlayer.loopPointReached += OnCinematicaTerminada;
    }

    private void OnCinematicaTerminada(VideoPlayer vp)
    {
        if (videoPanelUI != null)
            videoPanelUI.SetActive(false);
        cinematicaYaVista = true;
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnCinematicaTerminada;
    }

    private void ActualizarSprite()
    {
        if (damageSprites == null || damageSprites.Length == 0) return;
        int index = Mathf.FloorToInt(
            (1f - (float)currentHP / 3f) * damageSprites.Length
        );
        index = Mathf.Clamp(index, 0, damageSprites.Length - 1);
        spriteRenderer.sprite = damageSprites[index];
    }

    private System.Collections.IEnumerator DestruirConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destruir();
    }

    private void Destruir()
    {
        if (componentePrefab != null)
        {
            Instantiate(componentePrefab, transform.position + dropOffset, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
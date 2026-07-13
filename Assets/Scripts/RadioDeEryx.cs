using UnityEngine;
using UnityEngine.Video;

public class RadioDeEryx : MonoBehaviour, IInteractable
{
    public bool UsesChopAnimation => cinematicaYaVista;

    [Header("Cinemática")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoPanelUI;

    [Header("Sprites de daño")]
    [SerializeField] private Sprite[] damageSprites;

    [Header("Drop")]
    [SerializeField] private GameObject componentePrefab;

    [Header("Timing")]
    [SerializeField] private float delayAntesDeDestruir = 0.2f; // 

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
        if (isDestruyendo) return; // 

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
        if (videoPanelUI != null)
            videoPanelUI.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnCinematicaTerminada;
        }
        else
        {
            OnCinematicaTerminada(null);
        }
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
            Vector3 offset = new Vector3(0.2f, -0.3f, 0f);
            Instantiate(componentePrefab, transform.position + offset, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
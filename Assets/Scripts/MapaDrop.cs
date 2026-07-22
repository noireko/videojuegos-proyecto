using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapaDrop : MonoBehaviour
{
    [Tooltip("Índice dentro del array 'imageObjects' de InteractionTextUI que corresponde al mensaje del mapa.")]
    [SerializeField] private int imagenIndex = 1;

    [Tooltip("Nombre de la escena de Game Over. Dejar vacío para cerrar el juego.")]
    [SerializeField] private string gameOverScene = "";

    [SerializeField] private float bobAmplitude = 0.08f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 startPos;
    private bool recogido = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (recogido || !other.CompareTag("Player")) return;
        recogido = true;

        float delay = 2f;

        if (InteractionTextUI.instance != null)
        {
            InteractionTextUI.instance.Show(imagenIndex);
            delay = InteractionTextUI.instance.LifeTime;
        }
        else
        {
            Debug.LogWarning("MapaDrop: no se encontró InteractionTextUI en la escena.");
        }

        StartCoroutine(TerminarTras(delay));

        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
    }

    private IEnumerator TerminarTras(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        TerminarJuego();
    }

    private void TerminarJuego()
    {
        if (!string.IsNullOrEmpty(gameOverScene))
        {
            SceneManager.LoadScene(gameOverScene);
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

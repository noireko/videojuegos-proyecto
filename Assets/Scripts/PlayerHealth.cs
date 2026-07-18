using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    [SerializeField] Slider healthBar;

    [Header("Desgaste de Vida (lineal en el tiempo)")]
    [SerializeField] private float perdidaVidaPorSegundo = 1.11f; // 100 HP / 90 segundos = 1min30 de vida total

    [Header("Reserva de Emergencia")]
    [SerializeField] private GameObject vialPrefab;
    [SerializeField] private float umbralVidaBaja = 0.3f;
    [SerializeField] private int maxViales = 3;
    [SerializeField] private float cooldownEntreViales = 15f;

    [Header("Efectos Visuales - Resplandor de John")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color colorRegeneracion = new Color(0.4f, 1f, 0.4f);
    [SerializeField] private float velocidadGlow = 4f;
    [SerializeField] private float intensidadGlow = 0.7f;

    int currentHP;
    private Coroutine regenRoutine;
    private int vialesGenerados = 0;
    private float ultimoSpawnTime = -999f;
    private float acumuladorDesgaste = 0f;
    private Color colorOriginalJohn = Color.white;
    private bool enZonaPurificada = false;

    void Start()
    {
        currentHP = maxHP;
        ActualizarBarra();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            colorOriginalJohn = spriteRenderer.color;
    }

    void Update()
    {
        if (!enZonaPurificada)
        {
            acumuladorDesgaste += perdidaVidaPorSegundo * Time.deltaTime;
            if (acumuladorDesgaste >= 1f)
            {
                int danoEntero = Mathf.FloorToInt(acumuladorDesgaste);
                acumuladorDesgaste -= danoEntero;
                TakeDamage(danoEntero);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        ActualizarBarra();

        RevisarSiHayQueGenerarVial();

        if (currentHP <= 0)
            Morir();
    }

    // ---------- Regeneración gradual + resplandor aditivo ----------
    public void StartFastRegen(int totalAmount, float duration)
    {
        if (regenRoutine != null)
            StopCoroutine(regenRoutine);

        regenRoutine = StartCoroutine(FastRegenCoroutine(totalAmount, duration));
    }

    private IEnumerator FastRegenCoroutine(int totalAmount, float duration)
    {
        int startHP = currentHP;
        int targetHP = Mathf.Min(startHP + totalAmount, maxHP);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progreso = elapsed / duration;
            currentHP = (int)Mathf.Lerp(startHP, targetHP, progreso);
            ActualizarBarra();

            if (spriteRenderer != null)
            {
                float pulso = Mathf.Abs(Mathf.Sin(Time.time * velocidadGlow)) * intensidadGlow;
                Color brillo = colorOriginalJohn + colorRegeneracion * pulso;
                spriteRenderer.color = new Color(
                    Mathf.Clamp01(brillo.r),
                    Mathf.Clamp01(brillo.g),
                    Mathf.Clamp01(brillo.b),
                    colorOriginalJohn.a
                );
            }

            yield return null;
        }

        currentHP = targetHP;
        ActualizarBarra();
        regenRoutine = null;

        if (spriteRenderer != null)
            spriteRenderer.color = colorOriginalJohn;
    }

    // ---------- Spawn automático cuando la vida está baja ----------
    private void RevisarSiHayQueGenerarVial()
    {
        bool vidaEsBaja = currentHP <= maxHP * umbralVidaBaja;
        bool todaviaQuedanViales = vialesGenerados < maxViales;
        bool yaPasoElCooldown = Time.time >= ultimoSpawnTime + cooldownEntreViales;

        if (vidaEsBaja && todaviaQuedanViales && yaPasoElCooldown && vialPrefab != null)
        {
            GenerarVialCercaDeJohn();
        }
    }

    private void GenerarVialCercaDeJohn()
    {
        Vector2 offset = Random.insideUnitCircle.normalized * 1.5f;
        Vector3 posicionSpawn = transform.position + (Vector3)offset;

        Instantiate(vialPrefab, posicionSpawn, Quaternion.identity);

        vialesGenerados++;
        ultimoSpawnTime = Time.time;

        Debug.Log($"Vial de emergencia generado ({vialesGenerados}/{maxViales})");
    }

    // ---------- Zona del Purificador Parcial ----------
    public void SetEnZonaPurificada(bool valor)
    {
        enZonaPurificada = valor;
    }

    // ---------- Muerte / respawn ----------
    void Morir()
    {
        Debug.Log("Jugador murió, respawneando...");
        currentHP = maxHP;
        ActualizarBarra();

        if (RespawnManager.instance != null)
            RespawnManager.instance.Respawn();
        else
            Debug.LogWarning("RespawnManager.instance es null - revisar si el objeto RespawnManager está activo en la escena.");
    }

    void ActualizarBarra()
    {
        if (healthBar != null)
            healthBar.value = currentHP;
    }
}
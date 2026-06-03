using UnityEngine;

public class arbol : MonoBehaviour, IInteractable
{
    [SerializeField] private int maxHP = 8;
    [SerializeField] private GameObject woodDropPrefab;

    [Header("Sprites de daño")]
    [SerializeField] private Sprite[] damageSprites;

    private int currentHP;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateTreeSprite();
    }

    public string GetPrompt()
    {
        return "Talar árbol";
    }

    public void Interact()
    {
        currentHP--;

        Debug.Log($"Árbol golpeado. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            Derribar();
            return;
        }

        UpdateTreeSprite();
    }

    private void UpdateTreeSprite()
    {
        if (damageSprites == null || damageSprites.Length == 0)
            return;

        int spriteIndex = Mathf.FloorToInt(
            (1f - (float)currentHP / maxHP) * damageSprites.Length
        );

        spriteIndex = Mathf.Clamp(
            spriteIndex,
            0,
            damageSprites.Length - 1
        );

        spriteRenderer.sprite = damageSprites[spriteIndex];
    }

    private void Derribar()
    {
        if (woodDropPrefab != null)
        {
            int woodAmount = Random.Range(2, 4); // 2 o 3

            for (int i = 0; i < woodAmount; i++)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-0.3f, 0.3f),
                    Random.Range(-0.3f, 0.3f),
                    0f
                );

                Instantiate(
                    woodDropPrefab,
                    transform.position + offset,
                    Quaternion.identity
                );
            }
        }

        Destroy(gameObject);
    }
}
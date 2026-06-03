using UnityEngine;

public class arbol : MonoBehaviour, IInteractable
{
    [SerializeField] int maxHP = 3;
    [SerializeField] GameObject woodDropPrefab;

    int currentHP;

    void Start() => currentHP = maxHP;

    public string GetPrompt() => "Talar árbol";

    public void Interact()
    {
        currentHP--;
        Debug.Log($"Arbol golpeado. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
            Derribar();
    }

    void Derribar()
    {
        if (woodDropPrefab != null)
            Instantiate(woodDropPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    [SerializeField] Slider healthBar;

    int currentHP;

    void Start()
    {
        currentHP = maxHP;
        ActualizarBarra();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"Jugador recibió {amount} daño. HP: {currentHP}/{maxHP}");
        ActualizarBarra();

        if (currentHP <= 0)
            Morir();
    }

    void Morir()
    {
        Debug.Log("Jugador murió, respawneando...");
        currentHP = maxHP;
        ActualizarBarra();
        RespawnManager.instance.Respawn();
    }

    void ActualizarBarra()
    {
        if (healthBar != null)
            healthBar.value = currentHP;
    }
}
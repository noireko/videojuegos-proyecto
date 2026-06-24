using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"Jugador recibió {amount} daño. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
            Morir();
    }

    void Morir()
    {
        Debug.Log("Jugador murió, respawneando...");
        currentHP = maxHP;
        RespawnManager.instance.Respawn();
    }
}
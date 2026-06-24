using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public bool UsesChopAnimation => false;

    public string GetPrompt() => "Dormir";

    public void Interact()
    {
        RespawnManager.instance.SetSpawnPoint(transform.position);
        Debug.Log("Punto de respawn guardado en la cama.");
    }
}
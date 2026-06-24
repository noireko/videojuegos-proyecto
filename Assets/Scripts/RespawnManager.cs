using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    private Vector3 spawnPoint;
    private GameObject player;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = player.transform.position;
    }

    public void SetSpawnPoint(Vector3 position)
    {
        spawnPoint = position;
    }

    public void Respawn()
    {
        player.transform.position = spawnPoint;
    }
}
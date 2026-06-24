using UnityEngine;

public class InventoryWindowUI : MonoBehaviour
{
    [SerializeField] private GameObject constructionPanel;

    void Start()
    {
        constructionPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            bool abierto = constructionPanel.activeSelf;
            constructionPanel.SetActive(!abierto);
        }
    }
}
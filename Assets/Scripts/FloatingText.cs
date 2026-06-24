using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2.5f;
    [SerializeField] private float floatSpeed = 0.3f;
    private TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string message)
    {
        if (textMesh != null)
            textMesh.text = message;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }
}
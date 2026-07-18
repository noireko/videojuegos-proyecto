using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField] private Transform pivotSuelo;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        float y = pivotSuelo != null ? pivotSuelo.position.y : transform.position.y;
        spriteRenderer.sortingOrder = Mathf.RoundToInt(y * -100);
    }
}
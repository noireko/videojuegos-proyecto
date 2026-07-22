using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [System.Serializable]
    public class BuildCost
    {
        public string itemName;
        public int amount;
    }

    [System.Serializable]
    public class BuildItem
    {
        public string buildName;
        public GameObject buildPrefab;
        public List<BuildCost> costs = new List<BuildCost>();
    }

    [Header("Construcciones disponibles")]
    [SerializeField] private List<BuildItem> buildItems = new List<BuildItem>();

    [Header("Configuración")]
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask blockedLayer;
    [SerializeField] private Vector2 checkSize = new Vector2(0.8f, 0.8f);

    private BuildItem selectedItem;
    private GameObject previewObject;
    private SpriteRenderer[] previewRenderers;

    private bool isBuilding = false;

    void Update()
    {
        if (!isBuilding || selectedItem == null)
            return;

        UpdatePreviewPosition();

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            TryBuild();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuild();
        }
    }

    public void SelectBuildItem(int index)
    {
        if (index < 0 || index >= buildItems.Count)
            return;

        selectedItem = buildItems[index];
        isBuilding = true;

        CreatePreview();
    }

    private void CreatePreview()
    {
        if (previewObject != null)
            Destroy(previewObject);

        previewObject = Instantiate(selectedItem.buildPrefab);
        previewObject.name = "Build Preview";

        Collider2D[] colliders = previewObject.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        MonoBehaviour[] scripts = previewObject.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
                script.enabled = false;
        }

        previewRenderers = previewObject.GetComponentsInChildren<SpriteRenderer>();

        SetPreviewColor(true);
    }

    private void UpdatePreviewPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 snappedPosition = new Vector3(
            Mathf.Round(mouseWorldPos.x / gridSize) * gridSize,
            Mathf.Round(mouseWorldPos.y / gridSize) * gridSize,
            0f
        );

        previewObject.transform.position = snappedPosition;

        bool canBuild = CanBuild(snappedPosition);
        SetPreviewColor(canBuild);
    }

    private void TryBuild()
    {
        Vector3 buildPosition = previewObject.transform.position;

        if (!CanBuild(buildPosition))
        {
            Debug.Log("No se puede construir acá.");
            return;
        }

        if (!HasRequiredMaterials())
        {
            Debug.Log("No tenés materiales suficientes.");
            return;
        }

        SpendMaterials();

        Instantiate(
            selectedItem.buildPrefab,
            buildPosition,
            Quaternion.identity
        );

        Debug.Log($"Construido: {selectedItem.buildName}");
    }

    private bool CanBuild(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapBox(
            position,
            checkSize,
            0f,
            blockedLayer
        );

        return hit == null;
    }

    private bool HasRequiredMaterials()
    {
        foreach (BuildCost cost in selectedItem.costs)
        {
            if (!Inventory.instance.HasItem(cost.itemName, cost.amount))
                return false;
        }

        return true;
    }

    private void SpendMaterials()
    {
        foreach (BuildCost cost in selectedItem.costs)
        {
            Inventory.instance.RemoveItem(cost.itemName, cost.amount);
        }
    }

    private void SetPreviewColor(bool canBuild)
    {
        if (previewRenderers == null)
            return;

        Color color = canBuild
            ? new Color(1f, 1f, 1f, 0.5f)
            : new Color(1f, 0f, 0f, 0.5f);

        foreach (SpriteRenderer sr in previewRenderers)
        {
            sr.color = color;
        }
    }

    public void CancelBuild()
    {
        isBuilding = false;
        selectedItem = null;

        if (previewObject != null)
            Destroy(previewObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, checkSize);
    }
}
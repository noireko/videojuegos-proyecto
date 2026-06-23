using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName, int amount)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] += amount;
        }
        else
        {
            items.Add(itemName, amount);
        }

        Debug.Log($"{itemName}: {items[itemName]}");

        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui != null) ui.RefrescarUI();

    }

    public int GetItemAmount(string itemName)
    {
        if (items.ContainsKey(itemName))
            return items[itemName];

        return 0;
    }

    public bool HasItem(string itemName, int amount)
    {
        return GetItemAmount(itemName) >= amount;
    }

    public bool RemoveItem(string itemName, int amount)
    {
        if (!HasItem(itemName, amount))
            return false;

        items[itemName] -= amount;

        if (items[itemName] <= 0)
            items.Remove(itemName);

        return true;
    }
}
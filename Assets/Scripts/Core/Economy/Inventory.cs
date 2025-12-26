using UnityEngine;
using System.Collections.Generic;

public class Inventory
{
    public Dictionary<ResourceType, float> Stockpile { get; private set; }

    public Inventory()
    {
        Stockpile = new Dictionary<ResourceType, float>();
        InitializeStockpile();

    }

    private void InitializeStockpile()
    {
        // Add every resource defined in your ResourceType class
        Stockpile[ResourceType.Wood] = 0f;
        Stockpile[ResourceType.Iron] = 0f;
        Stockpile[ResourceType.Coal] = 0f;
        Stockpile[ResourceType.Wheat] = 0f;
    }

    public void Add(ResourceType resource, float amount)
    {
        if (resource != null)
        {
            Stockpile[resource] += amount;
        }
    }

    public void Remove(ResourceType resource, float amount)
    {
        if (resource != null)
        {
            Stockpile[resource] -= amount;
        }
    }


}

using UnityEngine;

public static class EconomicEngine
{
    public static void ProcessTick(Country country)
    {
        ExtractResources(country);
        ConsumeResources(country);
    }

    public static void ExtractResources(Country c)
    {
        foreach (HexTile tile in c.ClaimedTiles)
        {
            if (tile.Resource != null)
            {
                c.Inventory.Add(tile.Resource, tile.Resource.Yield);
            }
        }
    }

    public static void ConsumeResources(Country c)
    {
        float foodRequired = c.Stats.GetTotalFoodRequirement();
        c.Inventory.Remove(ResourceType.Wheat, foodRequired);
    }
}

using System;
using UnityEngine;

public sealed class WorldGenerator : MonoBehaviour
{
    public WorldGrid WorldGrid { get; private set; }
    [SerializeField] int width = 100;
    [SerializeField] int height = 100;
    public void Generate()
    {

        System.Random rnd = new System.Random();

        WorldGrid = new WorldGrid(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 2. Add Simple Terrain Logic
                int terrainRoll = rnd.Next(1, 4);
                TerrainType terrain = terrainRoll switch
                {
                    1 => TerrainType.Plains,
                    2 => TerrainType.Mountains,
                    _ => TerrainType.Woods
                };

                // 3. CRITICAL: Add Resources!
                // If it's Plains, give it Wheat. Otherwise null.
                ResourceType? res = (terrain == TerrainType.Plains) ? ResourceType.Wheat : null;

                WorldGrid.Set(x, y, new HexTile(terrain, res, null, x, y));
            }
        }
        // 4. Inject the Grid into the Manager
        // This connects the "Builder" to the "Manager"
        if (CountryManager.Instance != null)
        {
            CountryManager.Instance.SetGameMap(WorldGrid);

            SpawnCountry(1, 2, 2);
            SpawnCountry(2, 5, 5);

            CountryManager.Instance.RefreshCountryClaims();
        }
    }

    public void SpawnCountry(int countryId, int x, int y)
    {
        HexTile tile = WorldGrid.Get(x, y);

        if (tile != null)
        {
            tile.SetOwner(countryId);
        }
        else
        {
            Debug.LogError($"Failed to spawn country {countryId} at {x},{y}. Spot is outside the map!");
        }

    }
}

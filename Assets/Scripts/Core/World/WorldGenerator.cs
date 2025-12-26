using System;
using Mono.Cecil;
using UnityEngine;

public sealed class WorldGenerator : MonoBehaviour
{
    public WorldGrid WorldGrid { get; private set; }
    [SerializeField] int width = 10;
    [SerializeField] int height = 10;
    public void Generate()
    {

        System.Random rnd = new System.Random();

        WorldGrid = new WorldGrid(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int random = rnd.Next(1, 4);
                TerrainType randomTerrain;
                if (random == 1)
                {
                    randomTerrain = TerrainType.Plains;
                }
                else if (random == 2)
                {
                    randomTerrain = TerrainType.Mountains;
                }
                else
                {
                    randomTerrain = TerrainType.Woods;
                }

                WorldGrid.Set(x, y,
                new HexTile(
                    terrain: randomTerrain,
                    resource: null,
                    ownerCountryId: null,
                    x,
                    y
                    )
                );
            }
        }
        SpawnCountry(1, 2, 2);
        SpawnCountry(2, 5, 5);
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

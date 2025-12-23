using System;
using Unity.VisualScripting;
using UnityEngine;

public sealed class WorldGenerator : MonoBehaviour
{
    public WorldGrid WorldGrid { get; private set; }
    [SerializeField] int width = 10;
    [SerializeField] int height = 10;
    public void Generate()
    {

        WorldGrid = new WorldGrid(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                WorldGrid.Set(x, y,
                new HexTile(
                    terrain: TerrainType.Plains,
                    resource: null,
                    ownerCountryId: null,
                    x,
                    y
                    )
                );
            }
        }
    }
}

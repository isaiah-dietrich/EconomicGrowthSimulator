using System;
using System.Collections.Generic;
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
    }


}

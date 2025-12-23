using System;

public sealed class WorldGenerator
{
    public WorldGrid Generate(int width, int height)
    {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

        WorldGrid worldGrid = new WorldGrid(width, height);
        /*
        NOTE: X and Y Coordinates are not equivalent to actual locations
        */
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                worldGrid.Set(x, y, 
                new Region(
                    terrain: TerrainType.Plains,
                    resource: null,
                    ownerCountryId: null
                    )
                );
            }
        }
        return worldGrid;
    }
}

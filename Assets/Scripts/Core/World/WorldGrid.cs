using System;
using UnityEngine;

/*
Using Offset coordinates with a oddq grid type (Flat top)
*/
public sealed class WorldGrid
{

    public int Width { get; }
    public int Height { get; }

    private HexTile[,] Grid;

    public HexTile[,] GridData => Grid;

    public WorldGrid(int width, int height)
    {

        Grid = new HexTile[width, height];
        this.Width = width;
        this.Height = height;

    }

    /*
    NOTE: X and Y do not represent actual coordinates
    */
    public void Set(int x, int y, HexTile hexTile)
    {
        Grid[x, y] = hexTile;       
    }

    public HexTile Get(int x, int y)
    {
        return Grid[x, y];
    }

    //Returns the 6 neighbors of a given index
    public HexTile[] GetHexNeighbors(HexTile tile)
    {

        //First convert coordinates into cube system to make algorithm easier

        //Columns = q
        //Rows = r
        (int q, int r, int s) cube = OffsetToCube(tile);

        HexTile[] neighbors = new HexTile[6];
        // E, NE, NW, W, SW, SE
        (int dq, int dr, int ds)[] dirs = new[]
        {
        (+1,  0, -1), // E
        (+1, -1,  0), // NE
        ( 0, -1, +1), // NW
        (-1,  0, +1), // W
        (-1, +1,  0), // SW
        ( 0, +1, -1), // SE
        };

        for (int i = 0; i < 6; i++)
        {
            int nq = cube.q + dirs[i].dq;
            int nr = cube.r + dirs[i].dr;
            int ns = cube.s + dirs[i].ds;

            var (ncol, nrow) = CubeToOffset(nq, nr, ns);

            //Insures we are within array bounds
            if (ncol >= 0 && ncol < Width && nrow >= 0 && nrow < Height)
            {
                neighbors[i] = Grid[ncol, nrow];
            }
        }
        return neighbors;
    }

    //OddqOffset (col, row) -> Cube (q, r , s)
    public (int q, int r, int s) OffsetToCube(HexTile tile)
    {
        int q = tile.X;
        int r = tile.Y - (tile.X - (tile.X & 1)) / 2;
        int s = -q - r; // s + q + r = 0 so s = -q - r
        return (q, r, -q - r);
    }
    //Cube (q, r, s) -> OddqOffset (col, row)
    public (int col, int row) CubeToOffset(int q, int r, int s)
    {
        int col = q;
        int row = r + (q - (q & 1)) / 2;
        return (col, row);

    }


    public int DistanceToOtherTile(HexTile a, HexTile b)
    {
        //First convert to Cube
        var (qa, ra, sa) = OffsetToCube(a);
        var (qb, rb, sb) = OffsetToCube(b);
        //Manhattan Distance formula adjusted using Max Formula: https://www.geeksforgeeks.org/data-science/manhattan-distance/
        return Mathf.Max(Mathf.Abs(qa - qb), Mathf.Abs(ra - rb), Mathf.Abs(sa - sb));
    }
}

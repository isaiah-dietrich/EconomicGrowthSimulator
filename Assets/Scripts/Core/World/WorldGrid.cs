using UnityEngine;

/*
Using Offset coordinates with a oddq grid type (Flat top)
*/
public sealed class WorldGrid
{

    public int Width { get; }
    public int Height { get; }

    private Region[,] Grid;

    public WorldGrid(int width, int height)
    {

        Grid = new Region[width, height];

    }

    /*
    NOTE: X and Y do not represent actual coordinates
    */
    public void Set(int x, int y, Region region)
    {
        Grid[x, y] = region;
    }

    public void Get(int x, int y)
    {
        return Grid[x, y];
    }

    /*
    Get neighbors in a hexagon based system
    */
    public Region[] GetHexNeighbors(int x, int y)
    {

        //First convert coordinates into cube system to make algorithm easier

        //Columns = q
        //Rows = r
        (int q, int r, int s) cube = OddqOffsetToCube(x, y);

        Region[] neighbors = new Region[6];
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
            int nq = q  + dirs[i].dq;
            int nr = r + dirs[i].dr;
            int ns = s + dirs[i].ds;

            var (ncol, nrow) = CubeToOddqOffset(nq,nr,ns);
            neighbors[i] = Grid[ncol, nrow];
        }
    }

    /*
    Function to convert Coordinates from Odd-q Offset coords to Cube coords
    */
    public (int q, int r, int s) OddqOffsetToCube(int col, int row)
    {
        int q = col;
        int r = row - (col - (col & 1)) / 2;
        int s = -q - r; // s + q + r = 0 so s = -q - r
        return (q, r, -q - r);
    }

    public (int col, int row) CubeToOddqOffset(int q, int r, int s)
    {
        var col = q;
        var row = r + (q - (q & 1)) / 2;
        return (col, row);

    }












}

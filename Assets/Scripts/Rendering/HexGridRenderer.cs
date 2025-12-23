using System;
using UnityEngine;

public class HexGridRenderer : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;
    [SerializeField] GameObject TilePreFab; 
    //Out radius (Distance from center to corners)
    private static float HexSize = 1f;
    //In radius (Distance from center to edges) HexSize * cos(30) gives the radius of the inner circle
    private static readonly float InRadius =  HexSize *  (float) (Math.Sqrt(3) / 2);

    private static readonly float Width = 2 * HexSize;
    
    private static readonly float Height = (float) Math.Sqrt(3) * HexSize;

    private static readonly float HorizontalDistance = (0.75f) * Width;
    private static readonly float VerticalDistance = Height;

    
    public Vector3 GetWorldPosition(HexTile tile)
    {

        float xPos = tile.X * HorizontalDistance;
        float zPos = tile.Y * VerticalDistance;

        //If column is odd shift it by half the width (Radius)
        if ((tile.X & 1) != 0)
        {
            zPos += InRadius;
        }
        return new Vector3(xPos, 0, zPos);
    }

    public void GenerateTiles()
    {
        worldGenerator.Generate();

        foreach (HexTile tile in worldGenerator.WorldGrid.GridData)
        {
            Vector3 worldPosition = GetWorldPosition(tile);
            Instantiate(TilePreFab, worldPosition, Quaternion.identity, transform);
            
        }
    }

    private void Start()
    {
        GenerateTiles();
    }
}

using UnityEngine;

public class HexTileView : MonoBehaviour
{
    public HexTile Tile { get; private set; }

    public void Init(HexTile tile)
    {
        Tile = tile;
    }
}

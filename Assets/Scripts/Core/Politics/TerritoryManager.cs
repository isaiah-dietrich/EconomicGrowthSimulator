using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public sealed class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ClaimTile(HexTile tile, Country country)
    {
        if (country.Capital == null)
        {
            Debug.Log("Must have a capital before claiming a tile");
            return;
        }
        if (tile.OwnerCountryId == null)

            tile.OwnerCountryId = country.Id;
        else
            Debug.Log("Tile claimed by other country");
    }

    public void UnclaimTile(HexTile tile, Country country)
    {
        if (tile.OwnerCountryId == country.Id)
        {
            tile.OwnerCountryId = null;
            if (tile.Equals(country.Capital)) {
                country.SetCapital(null);
            }
        }
        else
        {
            Debug.Log("Tile is not claimed by this country");
        }

    }

    // New: Spawn using a tile, not x,y
    public bool SpawnCountryAtTile(Country country, HexTile tile)
    {
        if (country == null || tile == null) return false;

        // ✅ Capital already placed
        if (country.Capital != null)
        {
            Debug.Log($"Country {country.Name} already has a capital.");
            return false;
        }

        // ✅ Can't spawn on an already-owned tile
        if (tile.OwnerCountryId.HasValue)
        {
            Debug.Log("Cannot spawn capital on an owned tile.");
            return false;
        }
        // Optional: set capital if you want
        country.SetCapital(tile);
        
        // claim center + neighbors
        ClaimTile(tile, country);

        

        return true;
    }
}

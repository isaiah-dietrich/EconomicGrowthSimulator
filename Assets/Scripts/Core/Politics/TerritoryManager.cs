using UnityEngine;

public class TerritoryManager
{
    public void ClaimTile(HexTile tile, Country country)
    {
        if (tile.OwnerCountryId == null)
        {
            tile.OwnerCountryId = country.Id;
        }
        else
        {
            Debug.Log("Tile claimed by other country");
        }
    }

    public void UnclaimTile(HexTile tile, Country country)
    {
        if (tile.OwnerCountryId == country.Id)
        {
            tile.OwnerCountryId = null;
        } else
        {
            Debug.Log("Tile is not claimed by this country");
        }
    }

    public void SpawnCountry(int countryId, int x, int y)
    {
        HexTile tile = WorldGrid.Instance.Get(x, y);
        //Claim the tiles around 
        HexTile[] neighbors = WorldGrid.Instance.GetHexNeighbors(tile);

        if (tile != null)
        {
            tile.SetOwner(countryId);
            foreach (HexTile t in neighbors)
            {
                t.SetOwner(countryId);
            }
        }
        else
        {
            Debug.LogError($"Failed to spawn country {countryId} at {x},{y}. Spot is outside the map!");
        }
    }

    




}

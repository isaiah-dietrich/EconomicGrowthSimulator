

/*
A Region is a passive container of geographic and resource constraints that a country can activate through labor.
*/
using Unity.VisualScripting;

public class HexTile
{
    public TerrainType Terrain {get; }
    public ResourceType? Resource {get; }

    public int X { get; private set; }
    public int Y { get; private set; }
    
    /*
        Stores the ID of the country that owns the region or null
    */
    public int? OwnerCountryId {get; set;}

    public HexTile(TerrainType terrain, ResourceType? resource, int? ownerCountryId, int x, int y)
    {   
        this.Terrain = terrain;
        this.Resource = resource;
        this.OwnerCountryId = ownerCountryId;
        this.X = x;
        this.Y = y;
    }

    public bool IsOwned => OwnerCountryId != null;

    public void SetOwner(int? ownerCountryId) 
    {
        this.OwnerCountryId = ownerCountryId;
    }

}

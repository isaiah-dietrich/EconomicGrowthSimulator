

/*
A Region is a passive container of geographic and resource constraints that a country can activate through labor.
*/
public class Region
{
    public TerrainType Terrain {get; }
    public ResourceType? Resource {get; }
    /*
        Stores the ID of the country that owns the region or null
    */
    public int? OwnerCountryId {get; private set;}

    public Region(TerrainType terrain, ResourceType? resource, int? ownerCountryId)
    {   
        this.Terrain = terrain;
        this.Resource = resource;
        this.OwnerCountryId = ownerCountryId;
    }

    public bool IsOwned => OwnerCountryId != null;

    public void SetOwner(int? ownerCountryId) 
    {
        this.OwnerCountryId = ownerCountryId;
    }

}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Country
{
    public string Name { get; private set; }
    public Color Color { get; private set; }
    public int Id { get; private set; }

    public Inventory Inventory { get; private set; }
    public Demographics Stats { get; private set; }

    public List<HexTile> ClaimedTiles { get; private set; }
    public HexTile Capital { get; private set; }

    public Country(CountryDefinition def)
    {
        Name = def.CountryName;
        Color = def.Color;
        Id = def.Id;

        Inventory = new Inventory();
        Stats = new Demographics(def.StartingPopulation, 0.1f);
        ClaimedTiles = new List<HexTile>();
    }

    public void SetCapital(HexTile tile)
    {
        Capital = tile;
    }
}

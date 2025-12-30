using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Country
{
    public string Name { get; }
    public Color Color { get; }
    public int Id;

    public Inventory Inventory { get; private set; }

    public Demographics Stats { get; private set; }

    public List<HexTile> ClaimedTiles { get; private set; }

    public HexTile Capital { get; private set; }

    public Country(string name, Color color, int id, int startingPopulation)
    {
        this.Name = name;
        this.Color = color;
        this.Id = id;

        this.Inventory = new Inventory();
        this.Stats = new Demographics(startingPopulation, 0.1f);

        this.ClaimedTiles = new List<HexTile>();


    }


}

using UnityEngine;

public class TerrainType
{
    public string Name {get; }

    public int MovementCost { get; }

    public int BuildCost { get; }
    
    public TerrainType(string name, int movementCost, int buildCost)
    {
        this.Name = name;
        this.MovementCost = movementCost;
        this.BuildCost = buildCost;
    }

    public static readonly TerrainType Mountains = new TerrainType("Mountains", 10, 10);
    public static readonly TerrainType Plains = new TerrainType("Plains", 1, 1);
    public static readonly TerrainType Woods = new TerrainType("Woods", 3, 5);
}

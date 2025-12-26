using UnityEngine;

public class ResourceType
{
    public string? Name { get; }

    public float Yield { get; }

    public ResourceType(string? name, float yield)
    {
        this.Name = name;
        this.Yield = yield;
    }

    public static readonly ResourceType Wood = new("Wood", 5f);
    public static readonly ResourceType Iron = new("Iron", 1f);
    public static readonly ResourceType Coal = new("Coal", 3f);
    public static readonly ResourceType Wheat = new("Wheat", 100f);

}

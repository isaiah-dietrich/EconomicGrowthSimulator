using UnityEngine;
/*
    Interface for a resource
*/
public class ResourceType
{
    public string? Name { get; }

    public ResourceType(string? name)
    {
        this.Name = name;
    }

    public static readonly ResourceType Wood = new ("Wood");
    public static readonly ResourceType Iron = new ("Iron");
    public static readonly ResourceType Coal = new ("Coal");



                  
}

using UnityEngine;
[System.Serializable]
public class Country
{
    public string Name {get; }
    public Color Color { get; }
    public int Id;

    public Country(string name, Color color, int id)
    {
        this.Name = name;
        this.Color = color;
        this.Id = id;
    }
}

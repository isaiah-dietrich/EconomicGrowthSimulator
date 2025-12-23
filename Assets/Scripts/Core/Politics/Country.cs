using UnityEngine;

public class Country
{
    public string Name {get; }
    public int Population { get; }

    public Country(string name, int population)
    {
        this.Name = name;
        this.Population = population;
    }

    public Country(string name)
    {
        this.Name = name;
        this.Population = 2;
    }
}

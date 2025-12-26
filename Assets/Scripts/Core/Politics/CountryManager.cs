using System.Collections.Generic;
using UnityEngine;

public class CountryManager : MonoBehaviour
{
    // Singleton Instance
    public static CountryManager Instance { get; private set;}

    private Dictionary<int, Country> countries = new Dictionary<int, Country>();

    private void Awake()
    {
        Instance = this;
        InitializeCountries();
    }

    private void InitializeCountries()
    {
        AddCountry(new Country("USA", Color.blue, 1));
        AddCountry(new Country("China", Color.yellow, 2));
    }

    public void AddCountry(Country country) => countries[country.Id] = country;

    public Country GetCountry(int id)
    {
        if (countries.TryGetValue(id, out Country c))
        {
            return c;
        } else
        {
            return null;
        }
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CountryManager : MonoBehaviour
{
    // Singleton Instance
    public static CountryManager Instance { get; private set; }

    private Dictionary<int, Country> countries = new Dictionary<int, Country>();

    private WorldGrid worldGrid;

    private void OnEnable()
    {
        // Subscribe to the clock
        SimulationClock.OnTick += HandleGlobalTick;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SimulationClock.OnTick -= HandleGlobalTick;
    }

    private void HandleGlobalTick()
    {
        foreach (var pair in countries)
        {
            EconomicEngine.ProcessTick(pair.Value);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        InitializeCountries();
    }

    private void InitializeCountries()
    {
        AddCountry(new Country("USA", Color.blue, 1, 2));
        AddCountry(new Country("China", Color.yellow, 2, 2));
    }

    public void AddCountry(Country country) => countries[country.Id] = country;

    public Country GetCountry(int id)
    {
        if (countries.TryGetValue(id, out Country c))
        {
            return c;
        }
        else
        {
            return null;
        }
    }

    //Adds The countries hex tiles to its internal ClaimedTiles list
    public void RefreshCountryClaims()
    {
        foreach (var pair in countries) pair.Value.ClaimedTiles.Clear();

        foreach (HexTile tile in worldGrid.GridData)
        {
            // 1. Check if the OwnerCountryId HAS a value (is not null)
            if (tile.OwnerCountryId.HasValue)
            {
                // 2. Use .Value to convert the 'int?' to a regular 'int'
                int ownerId = tile.OwnerCountryId.Value;

                if (countries.TryGetValue(ownerId, out Country owner))
                {
                    owner.ClaimedTiles.Add(tile);
                }
            }
        }
    }

    public void SetGameMap(WorldGrid grid)
    {
        this.worldGrid = grid;
    }



}

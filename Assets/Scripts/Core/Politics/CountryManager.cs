// [CountryDatabase]
//      |
//      v
// [CountryDefinition]  --->  (selected by player)
//      |
//      v
// [CountryManager] creates
//      |
//      v
// [Country]  <---- simulated every tick

using System.Collections.Generic;
using UnityEngine;

public sealed class CountryManager : MonoBehaviour
{
    public static CountryManager Instance { get; private set; }

    [SerializeField] private List<CountryDefinition> countryDatabase = new();

    private readonly Dictionary<int, Country> countriesById = new();
    private readonly Dictionary<int, CountryDefinition> defsById = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Build quick lookup for definitions, catch duplicates early
        defsById.Clear();
        foreach (var def in countryDatabase)
        {
            if (def == null) continue;

            if (defsById.ContainsKey(def.Id))
                Debug.LogError($"Duplicate CountryDefinition Id {def.Id} in database.");

            else
                defsById.Add(def.Id, def);
        }
    }

    private void OnEnable()  => SimulationClock.OnTick += HandleGlobalTick;
    private void OnDisable() => SimulationClock.OnTick -= HandleGlobalTick;

    private void HandleGlobalTick()
    {
        foreach (var c in countriesById.Values)
            EconomicEngine.ProcessTick(c);
    }

    public IReadOnlyList<CountryDefinition> Database => countryDatabase;
    public IReadOnlyCollection<Country> AllCountries() => countriesById.Values;

    public bool Exists(int id) => countriesById.ContainsKey(id);

    public Country? GetCountry(int id) =>
        countriesById.TryGetValue(id, out var c) ? c : null;

    public CountryDefinition? GetDefinition(int id) =>
        defsById.TryGetValue(id, out var def) ? def : null;

    public Country CreateFromDefinition(CountryDefinition def)
    {
        if (def == null) { Debug.LogError("CountryDefinition is null"); return null; }

        if (countriesById.TryGetValue(def.Id, out var existing))
            return existing;

        var c = new Country(def);
        countriesById.Add(c.Id, c);
        return c;
    }

    public Country CreateFromId(int id)
    {
        var def = GetDefinition(id);
        if (def == null) { Debug.LogError($"No CountryDefinition with Id {id}"); return null; }
        return CreateFromDefinition(def);
    }

    public void RemoveCountry(int id)
    {
        countriesById.Remove(id);
    }
}

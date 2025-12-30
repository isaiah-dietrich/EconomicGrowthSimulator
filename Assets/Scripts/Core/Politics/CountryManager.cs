using System.Collections.Generic;
using UnityEngine;

public sealed class CountryManager : MonoBehaviour
{
    public static CountryManager Instance { get; private set; }

    [Header("Database (ScriptableObject)")]
    [SerializeField] private CountryDatabase countryDatabase; // option A: single asset

    // Runtime countries (spawned this play session)
    private readonly Dictionary<int, Country> countriesById = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()  => SimulationClock.OnTick += HandleGlobalTick;
    private void OnDisable() => SimulationClock.OnTick -= HandleGlobalTick;

    private void HandleGlobalTick()
    {
        foreach (var c in countriesById.Values)
            EconomicEngine.ProcessTick(c);
    }

    // Expose the list so UI can show selectable countries
    public IReadOnlyList<CountryDefinition> Database =>
        countryDatabase != null ? countryDatabase.Countries : new List<CountryDefinition>();

    public bool Exists(int id) => countriesById.ContainsKey(id);

    public Country GetCountry(int id) =>
        countriesById.TryGetValue(id, out var c) ? c : null;

    public IReadOnlyCollection<Country> AllCountries() => countriesById.Values;

    public Country CreateFromDefinition(CountryDefinition def)
    {
        if (def == null) { Debug.LogError("CountryDefinition is null"); return null; }

        if (countriesById.ContainsKey(def.Id))
        {
            Debug.LogError($"Duplicate country id {def.Id}. Country already exists.");
            return countriesById[def.Id];
        }

        var c = new Country(def); // uses your Country(CountryDefinition def) constructor
        countriesById.Add(c.Id, c);
        return c;
    }

    public Country CreateFromDefinitionId(int id)
    {
        if (countryDatabase == null) { Debug.LogError("CountryDatabase is not assigned."); return null; }

        var def = countryDatabase.GetById(id);
        if (def == null) { Debug.LogError($"No CountryDefinition with id {id} in database."); return null; }

        return CreateFromDefinition(def);
    }

    public void RemoveCountry(int id)
    {
        countriesById.Remove(id);
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerritoryEditorController : MonoBehaviour
{
    public enum ToolMode
    {
        None,
        SpawnCapital,
        Claim,
        Unclaim
    }

    [Header("UI")]
    [SerializeField] private TMP_Dropdown countryDropdown;

    [Header("Managers")]
    [SerializeField] private CountryManager countryManager;

    public ToolMode CurrentMode { get; private set; } = ToolMode.None;
    public CountryDefinition SelectedCountryDef { get; private set; }

    private void Start()
    {
        PopulateCountryDropdown();
        SetMode(ToolMode.None);
    }

    private void PopulateCountryDropdown()
    {
        countryDropdown.ClearOptions();

        var defs = countryManager.Database;
        var options = new List<string>();

        foreach (var def in defs)
            options.Add(def.CountryName);

        countryDropdown.AddOptions(options);

        if (defs.Count > 0)
            SelectedCountryDef = defs[0];

        countryDropdown.onValueChanged.AddListener(OnCountryChanged);
    }

    private void OnCountryChanged(int index)
    {
        SelectedCountryDef = countryManager.Database[index];
    }


    // Button hooks
    public void SetSpawnMode()
    {
        SetMode(ToolMode.SpawnCapital);
    }

    public void SetClaimMode()
    {
        SetMode(ToolMode.Claim);
    }

    public void SetUnclaimMode()
    {
        SetMode(ToolMode.Unclaim);
    }

    private void SetMode(ToolMode mode)
    {
        CurrentMode = mode;
        Debug.Log($"Editor Mode: {CurrentMode}");
    }
}


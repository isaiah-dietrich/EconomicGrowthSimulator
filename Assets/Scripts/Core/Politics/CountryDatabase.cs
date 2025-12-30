using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Country Database")]
public class CountryDatabase : ScriptableObject
{
    [SerializeField] private List<CountryDefinition> countries = new();

    public IReadOnlyList<CountryDefinition> Countries => countries;

    public CountryDefinition GetById(int id)
    {
        for (int i = 0; i < countries.Count; i++)
        {
            if (countries[i] != null && countries[i].Id == id)
                return countries[i];
        }
        return null;
    }

    public bool ContainsId(int id)
    {
        for (int i = 0; i < countries.Count; i++)
        {
            if (countries[i] != null && countries[i].Id == id)
                return true;
        }
        return false;
    }
}

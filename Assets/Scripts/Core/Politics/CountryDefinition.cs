using UnityEngine;

[CreateAssetMenu(menuName = "Game/Country Definition")]
public class CountryDefinition : ScriptableObject
{
    public int Id;
    public string CountryName;
    public Color Color = Color.white;
    public int StartingPopulation = 1000;
}

using UnityEngine;

public class Demographics
{
    // Layer 1: The physical count of people
    public int Population { get; set; }

    // How much 'Wheat' (Food) 1 person eats per simulation tick
    // Example: 0.1 means 10 people eat 1 unit of food per tick
    public float ConsumptionRate { get; set; }

    public Demographics(int startingPopulation, float consumptionRate = 0.1f)
    {
        this.Population = startingPopulation;
        this.ConsumptionRate = consumptionRate;
    }

    // This tells the CountryManager exactly how much to subtract from the Stockpile
    public float GetTotalFoodRequirement()
    {
        return Population * ConsumptionRate;
    }

}

using UnityEngine;

[System.Serializable]
public class HouseholdSector
{
    // ASSETS
    public float LaborSupply;   // Total working population
    public float Cash;          // Total savings/money held by people
    
    // BEHAVIOR
    public float PropensityToConsume = 0.8f; // They spend 80% of income, save 20%
}

[System.Serializable]
public class FirmSector
{
    // ASSETS
    public float CapitalStock;  // Factories/Machines
    public float Inventory;     // Goods produced but not yet sold
    public float Cash;          // Corporate cash reserves (for paying wages)
    
    // BEHAVIOR
    public float WageRate = 10f; // Average wage per unit of labor
}
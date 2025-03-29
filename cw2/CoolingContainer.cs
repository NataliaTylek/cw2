namespace cw2;

public class CoolingContainer : Container
{
    public string ProductType { get; }
    public double Temperature { get; }
    
    private static readonly Dictionary<string, double> Temperatures = new(){
        { "Bananas", 13.3 },
        { "Chocolate", 18.0 },
        { "Fish", 2.0 },
        { "Meat", -15.0 },
        { "Ice Cream", -18.0 },
        { "Frozen Pizza", -30.0 },
        { "Cheese", 7.2 },
        { "Sausages", 5.0 },
        { "Butter", 20.5 },
        { "Eggs", 19.0 }
    };


    public CoolingContainer(
        double maxCapacity,
        double ownWeight,
        double height,
        double depth,
        string productType,
        double temperature
    ) : base(ContainerType.Cooling, maxCapacity, ownWeight, height, depth){
        if (Temperatures.TryGetValue(productType, out var temp))
        {
            if (temperature >= temp)
            {
                Temperature = temperature;
                ProductType = productType;
            }
            else
            {
                throw new InvalidOperationException("This product is not suitable for this temperature.");

            }
        }
        else
        {
            throw new InvalidDataException("This product is not allowed.");
        }
    }

    public override void LoadCargo(double weight)
    {
        if (weight > MaxCapacity)
        {
            throw new OverfillException($"{Sn} - Overfill!");
        }
        
        Weight = weight;
    }

    public override void UnloadCargo()
    {
        Weight = 0;
    }
}   
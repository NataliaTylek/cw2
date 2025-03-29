namespace cw2;

public abstract class Container
{
    private static int ID = 1;
    public string Sn { get; set; }
    public double Weight { get; set; }
    public double MaxCapacity { get; }
    public double OwnWeight { get; }
    public double Height { get; }
    public double Depth { get; }

    protected Container(
        ContainerType type,
        double maxCapacity,
        double ownWeight,
        double height,
        double depth)
    {
        MaxCapacity = maxCapacity;
        OwnWeight = ownWeight;
        Height = height;
        Depth = depth;
        Sn = $"KON-{GetSerialNumberType(type)}-{ID++}";
    }

    private string GetSerialNumberType(ContainerType type){
        switch (type){
            case ContainerType.Liquid:
                return "L";
            case ContainerType.Gas:
                return "G";
            case ContainerType.Cooling:
                return "C";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public abstract void LoadCargo(double weight);
    public abstract void UnloadCargo();
}
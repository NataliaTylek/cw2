namespace cw2;

public class LiquidContainer : Container, IHazardNotifier
{
    public bool IsHazardous { get; }

    public LiquidContainer(
        double maxCapacity,
        double ownWeight,
        double height,
        double depth,
        bool isHazardous
    ) : base(ContainerType.Liquid, maxCapacity, ownWeight, height, depth){
        IsHazardous = isHazardous;
    }

    public override void LoadCargo(double weight){
        double limit = IsHazardous ? MaxCapacity * 0.5 : MaxCapacity * 0.9;

        if (weight > limit){
            NotifyHazard("Overfill!", Sn);
            throw new OverfillException($"{Sn} - Overfill!");
        }
        
        Weight = weight;
    }

    public override void UnloadCargo(){
        Weight = 0;
    }

    public void NotifyHazard(string message, string serialNumber){
        Console.WriteLine($"{serialNumber} - {message}");
    }
}
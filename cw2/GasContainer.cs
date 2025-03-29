namespace cw2;

public class GasContainer : Container, IHazardNotifier
{
    public double Pressure { get; }

    public GasContainer(
        double maxCapacity,
        double ownWeight,
        double height,
        double depth,
        double pressure
    ) : base(ContainerType.Gas, maxCapacity, ownWeight, height, depth) {
        Pressure = pressure;
    }

    public override void LoadCargo(double weight){
        if (weight > MaxCapacity){
            NotifyHazard("Overfill!", Sn);
            throw new OverfillException($"{Sn} - Overfill!");
        }

        Weight = weight;
    }

    public override void UnloadCargo(){
        Weight *= 0.05;
    }

    public void NotifyHazard(string message, string serialNumber){
        Console.WriteLine($"{serialNumber} - {message}");
    }
}
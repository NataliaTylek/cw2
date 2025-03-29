namespace cw2;

public class Ship
{
    public double MaxSpeed { get; } 
    public int MaxContainers { get; } 
    public double MaxWeight { get; }

    private List<Container> containers;

    public Ship(double maxSpeed, int maxContainers, double maxWeight){
        MaxSpeed = maxSpeed;
        MaxContainers = maxContainers;
        MaxWeight = maxWeight;
        containers = new List<Container>();
    }

    public List<Container> GetContainers(){
        return containers;
    }

    public double GetTotalWeight(){
        double total = 0;

        foreach (var container in containers){
            total += container.OwnWeight + container.Weight;
        }

        return total;
    }

    public void AddContainer(Container container){
        if (containers.Count >= MaxContainers){
            throw new InvalidOperationException("Too many containers.");
        }

        if (GetTotalWeight() + container.OwnWeight + container.Weight > MaxWeight)
        {
            throw new OverfillException("Too much weight.");
        }

        containers.Add(container);
    }

    public void AddContainers(List<Container> containersToAdd)
    {
        if (containers.Count + containersToAdd.Count >= MaxContainers)
        {
            throw new InvalidOperationException("Too many containers.");
        }

        foreach (var container in containersToAdd)
        {
            try
            {
                AddContainer(container);
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't add container to the list.");
                Console.WriteLine(e);
            }
        }
    }

    public void RemoveContainer(string serialNumber){
        Container containerToRemove = findContainer(serialNumber);
        containers.Remove(containerToRemove);
    }

    public void ReplaceContainer(string serialNumber, Container newContainer){
        Container oldContainer = findContainer(serialNumber);

        double currentTotalWeight = GetTotalWeight();
        double newWeight = newContainer.OwnWeight + newContainer.Weight;
        double oldWeight = oldContainer.OwnWeight + oldContainer.Weight;

        if ((currentTotalWeight - oldWeight + newWeight) > MaxWeight){
           throw new OverfillException("Overfill!.");
        }

        int index = containers.IndexOf(oldContainer);
        containers[index] = newContainer;

        Console.WriteLine($"Container {serialNumber} replaced by container {newContainer.Sn}");
    }

    public void TransferContainer(Ship targetShip, string serialNumber){
        Container containerToMove = findContainer(serialNumber);

        containers.Remove(containerToMove);

        try{
            targetShip.AddContainer(containerToMove);
            Console.WriteLine($"Container {serialNumber} transferred.");
        } catch (Exception ex){
            containers.Add(containerToMove);
            throw ex;
        }
    }

    public void PrintAllContainers()
    {
        Console.WriteLine("\n=== Ship Cargo Summary ===");

        if (containers.Count == 0)
        {
            Console.WriteLine("No containers on board.");
            return;
        }

        foreach (var container in containers)
        {
            Console.WriteLine($"\nContainer Type: {container.GetType().Name}");
            Console.WriteLine($"Serial Number: {container.Sn}");
            Console.WriteLine($"Tare Weight: {container.OwnWeight} kg");
            Console.WriteLine($"Current Load: {container.Weight} kg");
            Console.WriteLine($"Max Capacity: {container.MaxCapacity} kg");

            switch (container)
            {
                case GasContainer gas:
                    Console.WriteLine($"Pressure: {gas.Pressure} bar");
                    break;

                case LiquidContainer liquid:
                    Console.WriteLine($"Hazardous Material: {(liquid.IsHazardous ? "Yes" : "No")}");
                    break;

                case CoolingContainer fridge:
                    Console.WriteLine($"Product Type: {fridge.ProductType}");
                    Console.WriteLine($"Storage Temperature: {fridge.Temperature}°C");
                    break;

                default:
                    Console.WriteLine("No additional information available.");
                    break;
            }
        }

        Console.WriteLine("\n-------------------------------");
    }

    private Container findContainer(string serialNumber)
    {
        Container container = null;

        foreach (var cont in containers){
            if (cont.Sn == serialNumber){
                container = cont;
                break;
            }
        }

        if (container == null){
            throw new InvalidOperationException($"No such container on the ship.");
        }
        
        return container;
    }
}
    

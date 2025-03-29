namespace cw2;

class Program
{
    private static List<Ship> ships = new List<Ship>();
    private static List<Container> containers = new List<Container>();

    static void Main(string[] args)
    {
        RunConsoleInterface();
    }

    static void RunConsoleInterface()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Container Management System ===");

            Console.WriteLine("\nLista kontenerowców:");
            if (ships.Count == 0)
            {
                Console.WriteLine("Brak");
            }
            else
            {
                for (int i = 0; i < ships.Count; i++)
                {
                    Ship ship = ships[i];
                    Console.WriteLine(
                        $"{i + 1}. Statek {i + 1} (speed={ship.MaxSpeed}, maxContainerNum={ship.MaxContainers}, maxWeight={ship.MaxWeight})");
                }
            }

            Console.WriteLine("\nLista kontenerów:");
            if (containers.Count == 0)
            {
                Console.WriteLine("Brak");
            }
            else
            {
                for (int i = 0; i < containers.Count; i++)
                {
                    Container container = containers[i];
                    string type = container.GetType().Name;
                    Console.WriteLine(
                        $"{i + 1}. {type} - {container.Sn}, Load: {container.Weight}/{container.MaxCapacity} kg");
                }
            }

            Console.WriteLine("\nMożliwe akcje:");
            Console.WriteLine("1. Dodaj kontenerowiec");

            if (ships.Count > 0)
            {
                Console.WriteLine("2. Usuń kontenerowiec");
                Console.WriteLine("3. Dodaj kontener");
            }

            if (containers.Count > 0)
            {
                Console.WriteLine("4. Usuń kontener");
                Console.WriteLine("5. Załaduj ładunek do kontenera");
                Console.WriteLine("6. Rozładuj kontener");
            }

            if (ships.Count > 0 && containers.Count > 0)
            {
                Console.WriteLine("7. Załaduj kontener na statek");
                Console.WriteLine("8. Usuń kontener ze statku");
            }

            if (ships.Count > 1)
            {
                Console.WriteLine("9. Przenieś kontener między statkami");
            }

            if (ships.Count > 0)
            {
                Console.WriteLine("10. Wyświetl informacje o statku");
            }

            Console.WriteLine("0. Wyjście");

            Console.Write("\nWybierz opcję: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "0":
                    running = false;
                    break;
                case "1":
                    AddShip();
                    break;
                case "2":
                    if (ships.Count > 0) RemoveShip();
                    break;
                case "3":
                    if (ships.Count > 0) AddContainer();
                    break;
                case "4":
                    if (containers.Count > 0) RemoveContainer();
                    break;
                case "5":
                    if (containers.Count > 0) LoadContainerCargo();
                    break;
                case "6":
                    if (containers.Count > 0) UnloadContainer();
                    break;
                case "7":
                    if (ships.Count > 0 && containers.Count > 0) LoadContainerToShip();
                    break;
                case "8":
                    if (ships.Count > 0) RemoveContainerFromShip();
                    break;
                case "9":
                    if (ships.Count > 1) TransferContainerBetweenShips();
                    break;
                case "10":
                    if (ships.Count > 0) DisplayShipInfo();
                    break;
                default:
                    Console.WriteLine("Nieprawidłowa opcja. Naciśnij dowolny klawisz, aby kontynuować...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddShip()
    {
        Console.Clear();
        Console.WriteLine("=== Dodawanie nowego kontenerowca ===");

        try
        {
            Console.Write("Podaj maksymalną prędkość (w węzłach): ");
            double maxSpeed = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("Podaj maksymalną liczbę kontenerów: ");
            int maxContainers = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Podaj maksymalną wagę (w tonach): ");
            double maxWeight = double.Parse(Console.ReadLine() ?? "0") * 1000;

            Ship newShip = new Ship(maxSpeed, maxContainers, maxWeight);
            ships.Add(newShip);

            Console.WriteLine("\nKontenerowiec dodany pomyślnie!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nBłąd: {ex.Message}");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void RemoveShip()
    {
        Console.Clear();
        Console.WriteLine("=== Usuwanie kontenerowca ===");

        for (int i = 0; i < ships.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Statek {i + 1} (kontenery: {ships[i].GetContainers().Count})");
        }

        Console.Write("\nWybierz numer statku do usunięcia (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= ships.Count)
        {
            Ship shipToRemove = ships[index - 1];

            if (shipToRemove.GetContainers().Count > 0)
            {
                Console.WriteLine("\nStatek zawiera kontenery. Czy na pewno chcesz go usunąć? (T/N)");
                string confirm = Console.ReadLine()?.ToUpper() ?? "";

                if (confirm != "T")
                {
                    Console.WriteLine("Anulowano.");
                    Console.ReadKey();
                    return;
                }
            }

            ships.RemoveAt(index - 1);
            Console.WriteLine("\nKontenerowiec usunięty pomyślnie!");
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void AddContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Dodawanie nowego kontenera ===");
        Console.WriteLine("Wybierz typ kontenera:");
        Console.WriteLine("1. Kontener na płyny (L)");
        Console.WriteLine("2. Kontener na gaz (G)");
        Console.WriteLine("3. Kontener chłodniczy (C)");
        Console.Write("\nWybór: ");

        if (int.TryParse(Console.ReadLine(), out int containerType) && containerType >= 1 && containerType <= 3)
        {
            try
            {
                Console.Write("Podaj maksymalną ładowność (kg): ");
                double maxCapacity = double.Parse(Console.ReadLine() ?? "0");

                Console.Write("Podaj wagę własną kontenera (kg): ");
                double ownWeight = double.Parse(Console.ReadLine() ?? "0");

                Console.Write("Podaj wysokość (cm): ");
                double height = double.Parse(Console.ReadLine() ?? "0");

                Console.Write("Podaj głębokość (cm): ");
                double depth = double.Parse(Console.ReadLine() ?? "0");

                Container newContainer = null;

                switch (containerType)
                {
                    case 1:
                        Console.Write("Czy kontener będzie przewozić materiały niebezpieczne? (T/N): ");
                        bool isHazardous = (Console.ReadLine()?.ToUpper() ?? "") == "T";
                        newContainer = new LiquidContainer(maxCapacity, ownWeight, height, depth, isHazardous);
                        break;

                    case 2:
                        Console.Write("Podaj ciśnienie (w atmosferach): ");
                        double pressure = double.Parse(Console.ReadLine() ?? "0");
                        newContainer = new GasContainer(maxCapacity, ownWeight, height, depth, pressure);
                        break;

                    case 3:
                        Console.WriteLine("Dostępne typy produktów:");
                        Console.WriteLine("- Bananas (min temp: 13.3°C)");
                        Console.WriteLine("- Chocolate (min temp: 18.0°C)");
                        Console.WriteLine("- Fish (min temp: 2.0°C)");
                        Console.WriteLine("- Meat (min temp: -15.0°C)");
                        Console.WriteLine("- Ice Cream (min temp: -18.0°C)");
                        Console.WriteLine("- Frozen Pizza (min temp: -30.0°C)");
                        Console.WriteLine("- Cheese (min temp: 7.2°C)");
                        Console.WriteLine("- Sausages (min temp: 5.0°C)");
                        Console.WriteLine("- Butter (min temp: 20.5°C)");
                        Console.WriteLine("- Eggs (min temp: 19.0°C)");

                        Console.Write("\nPodaj typ produktu: ");
                        string productType = Console.ReadLine() ?? "";

                        Console.Write("Podaj temperaturę (°C): ");
                        double temperature = double.Parse(Console.ReadLine() ?? "0");

                        newContainer = new CoolingContainer(maxCapacity, ownWeight, height, depth, productType,
                            temperature);
                        break;
                }

                if (newContainer != null)
                {
                    containers.Add(newContainer);
                    Console.WriteLine($"\nKontener {newContainer.Sn} dodany pomyślnie!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nBłąd: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nNieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void RemoveContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Usuwanie kontenera ===");

        for (int i = 0; i < containers.Count; i++)
        {
            Container container = containers[i];
            Console.WriteLine($"{i + 1}. {container.Sn} - {container.GetType().Name}");
        }

        Console.Write("\nWybierz numer kontenera do usunięcia (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= containers.Count)
        {
            Container containerToRemove = containers[index - 1];
            bool isOnShip = false;

            foreach (var ship in ships)
            {
                if (ship.GetContainers().Contains(containerToRemove))
                {
                    isOnShip = true;
                    break;
                }
            }

            if (isOnShip)
            {
                Console.WriteLine("\nKontener znajduje się na statku. Najpierw usuń go ze statku.");
            }
            else
            {
                containers.RemoveAt(index - 1);
                Console.WriteLine("\nKontener usunięty pomyślnie!");
            }
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void LoadContainerCargo()
    {
        Console.Clear();
        Console.WriteLine("=== Załadunek kontenera ===");

        for (int i = 0; i < containers.Count; i++)
        {
            Container container = containers[i];
            Console.WriteLine(
                $"{i + 1}. {container.Sn} - {container.GetType().Name}, Ładunek: {container.Weight}/{container.MaxCapacity} kg");
        }

        Console.Write("\nWybierz numer kontenera do załadunku (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= containers.Count)
        {
            Container container = containers[index - 1];

            Console.Write($"Podaj wagę ładunku (kg, max: {container.MaxCapacity}): ");
            if (double.TryParse(Console.ReadLine(), out double weight))
            {
                try
                {
                    container.LoadCargo(weight);
                    Console.WriteLine($"\nZaładowano {weight}kg do kontenera {container.Sn}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nBłąd: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nNieprawidłowa wartość.");
            }
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void UnloadContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Rozładunek kontenera ===");

        for (int i = 0; i < containers.Count; i++)
        {
            Container container = containers[i];
            Console.WriteLine($"{i + 1}. {container.Sn} - {container.GetType().Name}, Ładunek: {container.Weight} kg");
        }

        Console.Write("\nWybierz numer kontenera do rozładunku (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= containers.Count)
        {
            Container container = containers[index - 1];
            container.UnloadCargo();
            Console.WriteLine($"\nKontener {container.Sn} rozładowany. Pozostały ładunek: {container.Weight} kg");
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void LoadContainerToShip()
    {
        Console.Clear();
        Console.WriteLine("=== Załadunek kontenera na statek ===");

        for (int i = 0; i < ships.Count; i++)
        {
            Ship ship = ships[i];
            Console.WriteLine(
                $"{i + 1}. Statek {i + 1} (kontenery: {ship.GetContainers().Count}/{ship.MaxContainers})");
        }

        Console.Write("\nWybierz numer statku (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int shipIndex) && shipIndex > 0 && shipIndex <= ships.Count)
        {
            Ship selectedShip = ships[shipIndex - 1];

            List<Container> availableContainers = new List<Container>();
            foreach (var container in containers)
            {
                if (!selectedShip.GetContainers().Contains(container))
                {
                    availableContainers.Add(container);
                }
            }

            if (availableContainers.Count == 0)
            {
                Console.WriteLine("\nBrak dostępnych kontenerów do załadunku.");
            }
            else
            {
                Console.WriteLine("\nDostępne kontenery:");
                for (int i = 0; i < availableContainers.Count; i++)
                {
                    Container container = availableContainers[i];
                    Console.WriteLine(
                        $"{i + 1}. {container.Sn} - {container.GetType().Name}, Waga: {container.OwnWeight + container.Weight} kg");
                }

                Console.Write("\nWybierz numer kontenera do załadunku (0 aby anulować): ");
                if (int.TryParse(Console.ReadLine(), out int containerIndex) && containerIndex > 0 &&
                    containerIndex <= availableContainers.Count)
                {
                    Container selectedContainer = availableContainers[containerIndex - 1];

                    try
                    {
                        selectedShip.AddContainer(selectedContainer);
                        Console.WriteLine($"\nKontener {selectedContainer.Sn} załadowany na statek {shipIndex}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nBłąd: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
                }
            }
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void RemoveContainerFromShip()
    {
        Console.Clear();
        Console.WriteLine("=== Usuwanie kontenera ze statku ===");

        for (int i = 0; i < ships.Count; i++)
        {
            Ship ship = ships[i];
            Console.WriteLine($"{i + 1}. Statek {i + 1} (kontenery: {ship.GetContainers().Count})");
        }

        Console.Write("\nWybierz numer statku (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int shipIndex) && shipIndex > 0 && shipIndex <= ships.Count)
        {
            Ship selectedShip = ships[shipIndex - 1];
            List<Container> shipContainers = selectedShip.GetContainers();

            if (shipContainers.Count == 0)
            {
                Console.WriteLine("\nBrak kontenerów na statku.");
            }
            else
            {
                Console.WriteLine("\nKontenery na statku:");
                for (int i = 0; i < shipContainers.Count; i++)
                {
                    Container container = shipContainers[i];
                    Console.WriteLine($"{i + 1}. {container.Sn} - {container.GetType().Name}");
                }

                Console.Write("\nWybierz numer kontenera do usunięcia (0 aby anulować): ");
                if (int.TryParse(Console.ReadLine(), out int containerIndex) && containerIndex > 0 &&
                    containerIndex <= shipContainers.Count)
                {
                    Container selectedContainer = shipContainers[containerIndex - 1];

                    try
                    {
                        selectedShip.RemoveContainer(selectedContainer.Sn);
                        Console.WriteLine($"\nKontener {selectedContainer.Sn} usunięty ze statku {shipIndex}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nBłąd: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
                }
            }
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void TransferContainerBetweenShips()
    {
        Console.Clear();
        Console.WriteLine("=== Przenoszenie kontenera między statkami ===");

        for (int i = 0; i < ships.Count; i++)
        {
            Ship ship = ships[i];
            Console.WriteLine($"{i + 1}. Statek {i + 1} (kontenery: {ship.GetContainers().Count})");
        }

        Console.Write("\nWybierz numer statku źródłowego (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int sourceIndex) && sourceIndex > 0 && sourceIndex <= ships.Count)
        {
            Ship sourceShip = ships[sourceIndex - 1];
            List<Container> shipContainers = sourceShip.GetContainers();

            if (shipContainers.Count == 0)
            {
                Console.WriteLine("\nBrak kontenerów na statku źródłowym.");
            }
            else
            {
                Console.WriteLine("\nKontenery na statku źródłowym:");
                for (int i = 0; i < shipContainers.Count; i++)
                {
                    Container container = shipContainers[i];
                    Console.WriteLine($"{i + 1}. {container.Sn} - {container.GetType().Name}");
                }

                Console.Write("\nWybierz numer kontenera do przeniesienia (0 aby anulować): ");
                if (int.TryParse(Console.ReadLine(), out int containerIndex) && containerIndex > 0 &&
                    containerIndex <= shipContainers.Count)
                {
                    Container selectedContainer = shipContainers[containerIndex - 1];

                    Console.WriteLine("\nWybierz statek docelowy:");
                    for (int i = 0; i < ships.Count; i++)
                    {
                        if (i != sourceIndex - 1)
                        {
                            Ship ship = ships[i];
                            Console.WriteLine(
                                $"{i + 1}. Statek {i + 1} (kontenery: {ship.GetContainers().Count}/{ship.MaxContainers})");
                        }
                    }

                    Console.Write("\nWybierz numer statku docelowego (0 aby anulować): ");
                    if (int.TryParse(Console.ReadLine(), out int targetIndex) && targetIndex > 0 &&
                        targetIndex <= ships.Count && targetIndex != sourceIndex)
                    {
                        Ship targetShip = ships[targetIndex - 1];

                        try
                        {
                            sourceShip.TransferContainer(targetShip, selectedContainer.Sn);
                            Console.WriteLine(
                                $"\nKontener {selectedContainer.Sn} przeniesiony ze statku {sourceIndex} na statek {targetIndex}.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nBłąd: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
                    }
                }
                else
                {
                    Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
                }
            }
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void DisplayShipInfo()
    {
        Console.Clear();
        Console.WriteLine("=== Informacje o statku ===");

        for (int i = 0; i < ships.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Statek {i + 1}");
        }

        Console.Write("\nWybierz numer statku (0 aby anulować): ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= ships.Count)
        {
            Ship ship = ships[index - 1];

            Console.WriteLine($"\nInformacje o statku {index}:");
            Console.WriteLine($"Maksymalna prędkość: {ship.MaxSpeed} węzłów");
            Console.WriteLine($"Maksymalna liczba kontenerów: {ship.MaxContainers}");
            Console.WriteLine($"Maksymalna waga: {ship.MaxWeight} kg");
            Console.WriteLine($"Aktualna liczba kontenerów: {ship.GetContainers().Count}");
            Console.WriteLine($"Aktualna waga: {ship.GetTotalWeight()} kg");

            ship.PrintAllContainers();
        }
        else
        {
            Console.WriteLine("\nAnulowano lub nieprawidłowy wybór.");
        }

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }
}
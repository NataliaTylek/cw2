namespace cw2;

public interface IHazardNotifier
{
    void NotifyHazard(string message, string serialNumber);
}
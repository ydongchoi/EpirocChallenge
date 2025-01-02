namespace MiningVehicle.API.Services
{
    public interface IVehicleService
    {
        Task AdjustSpeed(int speed);
        void Break();
        void ChargeBattery();
    }
}
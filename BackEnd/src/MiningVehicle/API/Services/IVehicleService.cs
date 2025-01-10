namespace MiningVehicle.API.Services
{
    public interface IVehicleService
    {
        Task AdjustSpeedAsync(int speed);
        Task BreakAsync();
        Task ChargeBatteryAsync();
        Task StopBatteryChargingAsync();
    }
}
namespace MiningVehicle.VehicleEmulator
{
    public interface IMiningVehicleEmulator{
        public Task StartEngineAsync();
        public Task StopEngineAsync();

        public Task AdjustSpeedAsync(int speed);
        public Task BreakAsync();
        
        public Task ChargeBatteryAsync();
        public Task StopBatteryChargingAsync();
    }
}
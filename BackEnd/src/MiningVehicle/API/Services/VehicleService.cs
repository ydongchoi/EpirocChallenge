using MiningVehicle.VehicleEmulator;

namespace MiningVehicle.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IMiningVehicleEmulator _miningVehicleEmulator;

        public VehicleService(IMiningVehicleEmulator miningVehicleEmulator)
        {
            _miningVehicleEmulator = miningVehicleEmulator;
        }

        public async Task AdjustSpeedAsync(int speed)
        {
            switch (speed)
            {
                // TODO : Refactor this to use an enum
                case -1:
                    await _miningVehicleEmulator.StopEngineAsync();
                    return;
                case 0:
                    await _miningVehicleEmulator.StartEngineAsync();
                    break;  
            }

            await _miningVehicleEmulator.AdjustSpeedAsync(speed);
        }

        public async Task BreakAsync()
        {
            await _miningVehicleEmulator.BreakAsync();
        }

        public async Task ChargeBatteryAsync()
        {
            await _miningVehicleEmulator.StopEngineAsync();
            
            await _miningVehicleEmulator.ChargeBatteryAsync();
        }

        public async Task StopBatteryChargingAsync()
        {
            await _miningVehicleEmulator.StopBatteryChargingAsync();
        }
    }
}
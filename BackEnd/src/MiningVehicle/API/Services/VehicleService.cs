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

        public async Task AdjustSpeed(int speed)
        {
            switch (speed)
            {
                // TODO : Refactor this to use an enum
                case -1:
                    _miningVehicleEmulator.StopEngine();
                    break;
                case 0:
                    _miningVehicleEmulator.StartEngine();
                    break;
            }

            await _miningVehicleEmulator.AdjustSpeed(speed);
        }

        public void Break()
        {
            _miningVehicleEmulator.Break();
        }

        public void ChargeBattery()
        {
            _miningVehicleEmulator.StopEngine();
            _miningVehicleEmulator.ChargeBattery();
        }

        public void StopBatteryCharging()
        {
            _miningVehicleEmulator.StopBatteryCharging();
        }
    }
}
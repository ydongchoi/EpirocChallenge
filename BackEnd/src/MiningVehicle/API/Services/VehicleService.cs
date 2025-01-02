
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
            if(speed == 0)
            {
                _miningVehicleEmulator.StartEngine();
            }
            else
            {
                await _miningVehicleEmulator.AdjustSpeed(speed);
            }
        }

        public void Break()
        {
            _miningVehicleEmulator.Break();
        }

        public void ChargeBattery()
        {
            _miningVehicleEmulator.ChargeBattery();
        }
    }
}
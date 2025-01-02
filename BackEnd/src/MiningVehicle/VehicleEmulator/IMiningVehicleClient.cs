using MiningVehicle.VehicleEmulator.Models;

namespace MiningVehicle.VehicleEmulator
{
    public interface IMiningVehicleClient
    {
        public Task ConnectAsync();
        public Task SendVehicleDataAsync(VehicleData vehicleData);
        public Task DisConnectAsync();
    }
}
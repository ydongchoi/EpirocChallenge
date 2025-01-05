using Microsoft.AspNetCore.SignalR.Client;
using MiningVehicle.VehicleEmulator.Models;

namespace MiningVehicle.VehicleEmulator
{
    public class MiningVehicleClient : IMiningVehicleClient
    {
        private readonly HubConnection _connection;

        public MiningVehicleClient(HubConnection connection)
        {
            _connection = connection;
        }

        public async Task ConnectAsync()
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("SignalR client connected.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        public async Task SendVehicleDataAsync(VehicleData vehicleData)
        {
            try
            {
                await _connection.InvokeAsync("SendVehicleDataAsync", vehicleData);
                Console.WriteLine($"Sended vehicle data...");

                _connection.On<VehicleData>("ReceiveVehicleData", (vehicleData) =>
                {
                    Console.WriteLine($"Recevied From Client Successfully: {vehicleData}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        public async Task DisConnectAsync()
        {
            await _connection.StopAsync();
            Console.WriteLine("SignalR client disconnected.\n");
        }

    }
}
using System.Timers;
using Microsoft.AspNetCore.SignalR.Client;
using MiningVehicle.VehicleEmulator.Models;

namespace MiningVehicle.VehicleEmulator
{
    public class MiningVehicleClient : IMiningVehicleClient
    {
        private readonly HubConnection _connection;
        private System.Timers.Timer _timer;

        public MiningVehicleClient(HubConnection connection)
        {
            _connection = connection;

            _timer = new System.Timers.Timer(30000);
            _timer.Elapsed += OnTimedEvent;

        }

        public async Task ConnectAsync()
        {
            try
            {
                if(_connection.State == HubConnectionState.Connected) return;

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

        private void OnTimedEvent(object? source, ElapsedEventArgs e)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                try
                {
                    _connection.InvokeAsync("Ping").Wait();
                    Console.WriteLine("Ping sent to server.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ping failed: {ex.Message}");
                }
            }
        }

    }
}
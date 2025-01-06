using System.Timers;
using Microsoft.AspNetCore.SignalR.Client;
using MiningVehicle.Infrastructure.Repositories;
using MiningVehicle.VehicleEmulator.Models;

namespace MiningVehicle.VehicleEmulator
{
    public class MiningVehicleClient : IMiningVehicleClient
    {
        private readonly HubConnection _connection;

        private readonly IRepository _vehicleDataRepository;
        private List<MiningVehicle.Infrastructure.Models.VehicleData> _vehicleData;
        
        private System.Timers.Timer _timer;

        public MiningVehicleClient(HubConnection connection, IRepository vehicleDataRepository)
        {
            _connection = connection;

            _vehicleDataRepository = vehicleDataRepository;
            _vehicleData = new List<MiningVehicle.Infrastructure.Models.VehicleData>();

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

                _connection.On<VehicleData>("ReceiveVehicleData", async (vehicleData) =>
                {
                    await SaveDataToVehicleRepository(vehicleData);
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

        private async Task SaveDataToVehicleRepository(VehicleData vehicleData){
            // Save vehicle data to database
            var vehicleDataInfrastructure = new MiningVehicle.Infrastructure.Models.VehicleData
            {
                TimeStamp = vehicleData.Timestamp,
                MotorData = new MiningVehicle.Infrastructure.Models.MotorData
                {
                    Status = vehicleData.MotorData.Status,
                    Rpm = vehicleData.MotorData.Rpm,
                    Speed = vehicleData.MotorData.Speed,
                    GearRatio = vehicleData.MotorData.GearRatio
                },
                BatteryData = new MiningVehicle.Infrastructure.Models.BatteryData
                {
                    Status = vehicleData.BatteryData.Status,
                    Capacity = vehicleData.BatteryData.Capacity,
                    Charge = vehicleData.BatteryData.Charge,
                    ChargingRate = vehicleData.BatteryData.ChargingRate,
                    Efficiency = vehicleData.BatteryData.Efficiency,
                    Percentage = vehicleData.BatteryData.Percentage,
                    Power = vehicleData.BatteryData.Power,                   
                    Temperature = vehicleData.BatteryData.Temperature
                }
            };

            _vehicleData.Add(vehicleDataInfrastructure);
            Console.WriteLine($"{_vehicleData.Count} vehicle data saved to list...");

            if(_vehicleData.Count > 50){
                await _vehicleDataRepository.AddVehicleDataAsync(_vehicleData);
                Console.WriteLine("Saved vehicle data to database...");
                _vehicleData.Clear();
            }
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
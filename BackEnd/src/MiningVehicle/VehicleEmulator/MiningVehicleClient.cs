using System.Timers;
using Microsoft.AspNetCore.SignalR.Client;
using MiningVehicle.Infrastructure.Repositories;
using MiningVehicle.Logger;
using MiningVehicle.VehicleEmulator.Models;

namespace MiningVehicle.VehicleEmulator
{
    public class MiningVehicleClient : IMiningVehicleClient
    {
        private readonly HubConnection _connection;
        private readonly IRepository _vehicleDataRepository;
        private readonly ILoggerManager _logger;
        private List<MiningVehicle.Infrastructure.Models.VehicleData> _vehicleData;
        private System.Timers.Timer _timer;

        public MiningVehicleClient(
            HubConnection connection,
            IRepository vehicleDataRepository,
            ILoggerManager logger)
        {
            _connection = connection;
            _vehicleDataRepository = vehicleDataRepository;
            _logger = logger;
            _vehicleData = new List<MiningVehicle.Infrastructure.Models.VehicleData>();

            _timer = new System.Timers.Timer(20000);
            _timer.Elapsed += OnTimedEvent;

            Console.WriteLine("Timer started.");
            _timer.Start();
        }

        public async Task ConnectAsync()
        {
            if (_connection.State == HubConnectionState.Connected) return;

            // TODO: Try Catch
            await _connection.StartAsync();
            _logger.LogInformation("SignalR client connected.");
        }

        public async Task SendVehicleDataAsync(VehicleData vehicleData)
        {
            try
            {
                await _connection.InvokeAsync("SendVehicleDataAsync", vehicleData);
                _logger.LogInformation("Sent vehicle data...");

                _connection.On<VehicleData>("ReceiveVehicleData", async (data) =>
                {
                    await SaveDataToVehicleRepository(data);
                    _logger.LogInformation($"Received From Client Successfully: {data}");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            await _connection.StopAsync();
            _logger.LogInformation("SignalR client disconnected.");
        }

        private async Task SaveDataToVehicleRepository(VehicleData vehicleData)
        {
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
                },
                BrakeData = new MiningVehicle.Infrastructure.Models.BrakeData
                {
                    Status = vehicleData.BreakData.Status
                }
            };

            _vehicleData.Add(vehicleDataInfrastructure);

            if (_vehicleData.Count > 50)
            {
                await _vehicleDataRepository.AddVehicleDataAsync(_vehicleData);
                _logger.LogInformation("Saved vehicle data to database...");

                _vehicleData.Clear();
            }
        }

        private void OnTimedEvent(object? source, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer event triggered.");    
            if (_connection.State == HubConnectionState.Connected)
            {
                // TODO: Try Catch
                Console.WriteLine("Ping sent to server.");

                _connection.InvokeAsync("Ping").Wait();
                _logger.LogInformation("Ping sent to server.");
            }
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using MiningVehicle.Infrastructure.Repositories;
using MiningVehicle.Logger;
using MiningVehicle.SignalR.VehicleHub.Models;
using System;
using System.Threading.Tasks;

namespace MiningVehicle.SignalR.VehicleHub
{
    public class VehicleDataHub : Hub
    {
        private readonly IRepository _vehicleDataRepository;
        private readonly ILoggerManager _logger;

        public VehicleDataHub(
            IRepository vehicleDataRepository,
            ILoggerManager logger)
        {
            _vehicleDataRepository = vehicleDataRepository;
            _logger = logger;
        }

        public string GetConnectionId() => Context.ConnectionId;

        public async Task SendVehicleDataAsync(VehicleData vehicleData)
        {
            try
            {
                LogVehicleData(vehicleData);

                await SendVehicleDataToUIAsync(vehicleData);

                await Clients.Caller.SendAsync("ReceiveVehicleData", vehicleData);
                _logger.LogInformation($"Sent vehicle data to caller with ConnectionId: {Context.ConnectionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendVehicleDataAsync: {ex.Message}");
                throw;
            }
        }

        private void LogVehicleData(VehicleData vehicleData)
        {
            _logger.LogInformation("Received vehicle data...");
            _logger.LogInformation($"Timestamp: {vehicleData.Timestamp}");
            _logger.LogInformation($"Status: {vehicleData.MotorData.Status}");
            _logger.LogInformation($"Rpm: {vehicleData.MotorData.Rpm}");
            _logger.LogInformation($"Speed: {vehicleData.MotorData.Speed}");
            _logger.LogInformation($"Gear Ratio: {vehicleData.MotorData.GearRatio}");
            _logger.LogInformation($"Battery Status: {vehicleData.BatteryData.Status}");
            _logger.LogInformation($"Battery Capacity: {vehicleData.BatteryData.Capacity}");
            _logger.LogInformation($"Battery Charge: {vehicleData.BatteryData.Charge}");
            _logger.LogInformation($"Battery Charging Rate: {vehicleData.BatteryData.ChargingRate}");
            _logger.LogInformation($"Battery Efficiency: {vehicleData.BatteryData.Efficiency}");
            _logger.LogInformation($"Battery Percentage: {vehicleData.BatteryData.Percentage}");
            _logger.LogInformation($"Battery Power: {vehicleData.BatteryData.Power}");
            _logger.LogInformation($"Battery Temperature: {vehicleData.BatteryData.Temperature}");
        }

        public async Task SendVehicleDataToUIAsync(VehicleData vehicleData)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveVehicleDataAsync", vehicleData);
                _logger.LogInformation("Sent vehicle data to UI...");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendVehicleDataToUIAsync: {ex.Message}");
                throw;
            }
        }

        public Task Ping()
        {
            _logger.LogInformation($"Ping received from client with ConnectionId: {Context.ConnectionId}");
            return Task.CompletedTask;
        }
    }
}
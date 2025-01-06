using Microsoft.AspNetCore.SignalR;
using MiningVehicle.Infrastructure.Repositories;
using MiningVehicle.SignalR.VehicleHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiningVehicle.SignalR.VehicleHub
{
    public class VehicleDataHub : Hub
    {
        private readonly IRepository _vehicleDataRepository;

        public VehicleDataHub(IRepository vehicleDataRepository)
        {
            _vehicleDataRepository = vehicleDataRepository;
        }

        public string GetConnectionId() => Context.ConnectionId;

        public async Task SendVehicleDataAsync(VehicleData vehicleData)
        {
            LogVehicleData(vehicleData);

            await SendVehicleDataToUIAsync(vehicleData);

            await Clients.Caller.SendAsync("ReceiveVehicleData", vehicleData);
            Console.WriteLine("Sent vehicle data to caller...");
        }

        private void LogVehicleData(VehicleData vehicleData)
        {
            Console.WriteLine("Received vehicle data...");
            Console.WriteLine($"Timestamp: {vehicleData.Timestamp}");
            Console.WriteLine($"Status: {vehicleData.MotorData.Status}");
            Console.WriteLine($"Rpm: {vehicleData.MotorData.Rpm}");
            Console.WriteLine($"Speed: {vehicleData.MotorData.Speed}");
            Console.WriteLine($"Gear Ratio: {vehicleData.MotorData.GearRatio}");
            Console.WriteLine($"Battery Status: {vehicleData.BatteryData.Status}");
            Console.WriteLine($"Battery Capacity: {vehicleData.BatteryData.Capacity}");
            Console.WriteLine($"Battery Charge: {vehicleData.BatteryData.Charge}");
            Console.WriteLine($"Battery Charging Rate: {vehicleData.BatteryData.ChargingRate}");
            Console.WriteLine($"Battery Efficiency: {vehicleData.BatteryData.Efficiency}");
            Console.WriteLine($"Battery Percentage: {vehicleData.BatteryData.Percentage}");
            Console.WriteLine($"Battery Power: {vehicleData.BatteryData.Power}");
            Console.WriteLine($"Battery Temperature: {vehicleData.BatteryData.Temperature}");
        }

        public async Task SendVehicleDataToUIAsync(VehicleData vehicleData)
        {
            await Clients.All.SendAsync("ReceiveVehicleDataAsync", vehicleData);
            Console.WriteLine("Sent vehicle data to UI...");
        }

        public Task Ping()
        {
            Console.WriteLine($"Ping received from client at {DateTime.UtcNow}");
            return Task.CompletedTask;
        }
    }
}
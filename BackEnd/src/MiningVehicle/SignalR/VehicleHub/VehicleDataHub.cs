using Microsoft.AspNetCore.SignalR;
using MiningVehicle.Infrastructure.Repositories;
using MiningVehicle.SignalR.VehicleHub.Models;

namespace MiningVehicle.SignalR.VehicleHub
{
    public class VehicleDataHub : Hub
    {
        private readonly IRepository _vehicleDataRepository;

        public VehicleDataHub(IRepository vehicleDataRepository)
        {
            _vehicleDataRepository = vehicleDataRepository;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task SendVehicleDataAsync(VehicleData vehicleData)
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
            
            await _vehicleDataRepository.AddVehicleDataAsync(vehicleDataInfrastructure);

            // Send vehicle data to UI
            await SendVehicleDataToUIAsync(vehicleData);

            await Clients.Caller.SendAsync("ReceiveVehicleData", vehicleData);
        }

        public async Task SendVehicleDataToUIAsync(VehicleData vehicleData)
        {
            await Clients.All.SendAsync("ReceiveVehicleDataAsync", vehicleData);
        }
    }
}
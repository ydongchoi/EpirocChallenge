using MiningVehicle.VehicleEmulator.Components;

namespace MiningVehicle.VehicleEmulator.Models
{
    public class VehicleData
    {
        public DateTime Timestamp { get; set; }
        public required BatteryData BatteryData { get; set; }
        public required BreakData BreakData { get; set; }
        public required MotorData MotorData { get; set; }
    }

    public class BatteryData
    {
        public double Capacity { get; set; }
        public double Charge { get; set; }
        public double ChargingRate { get; set; }
        public double Efficiency { get; set; }
        public double Percentage { get; set; }
        public double Power { get; set; }
        public BatteryStatus Status { get; set; }
        public double Temperature { get; set; }
    }

    public class BreakData{
        public BreakStatus Status { get; set; }
    }

    public class MotorData
    {
        public double GearRatio { get; set; }
        public MotorStatus Status { get; set; }
        public int Speed { get; set; }
        public int Rpm { get; set; }   
    }
}
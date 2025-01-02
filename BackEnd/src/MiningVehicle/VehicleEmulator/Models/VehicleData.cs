using MiningVehicle.VehicleEmulator.Components;

namespace MiningVehicle.VehicleEmulator.Models
{
    public class VehicleData
    {
        public DateTime Timestamp { get; set; }
        public BatteryData BatteryData { get; set; }
        public BreakData BreakData { get; set; }
        public MotorData MotorData { get; set; }
    }

    public class BatteryData
    {
        public double Capacity { get; set; }
        public double Charge { get; set; }
        public double ChargingRate { get; set; }
        public double Efficiency { get; set; }
        public double Percentage { get; set; }
        public BatteryStatus Status { get; set; }
        public double Temperature { get; set; }
    }

    public class BreakData{
        public BreakStatus? Status { get; set; }
    }

    public class MotorData
    {
        public double GearRatio { get; set; }
        public MotorStatus Status { get; set; }
        public int Speed { get; set; }
        public int Rpm { get; set; }   
    }
}
using Microsoft.Extensions.Options;
using MiningVehicle.VehicleEmulator.ConfigurationModels;

namespace MiningVehicle.VehicleEmulator.Components
{
    public class Battery
    {
        // Configuration
        private readonly IOptions<BatteryConfiguration> _batteryConfigurationOptions;
        private readonly BatteryConfiguration _batteryConfiguration;

        // Properties
        public double Capacity { get; private set; } // 60000 Wha
        public double Charge { get; private set; } // 50000 Wha (Fast Charger), take 30 minutes to charge at 80% capacity
        public double ChargingRate { get; private set; }
        public double Efficiency { get; private set; }
        public double Percentage => Charge / Capacity;
        public BatteryStatus Status { get; private set; }
        public double Temperature { get; private set; }

        // Constructor
        public Battery(IOptions<BatteryConfiguration> batteryConfigurationOptions)
        {
            _batteryConfigurationOptions = batteryConfigurationOptions;
            _batteryConfiguration = _batteryConfigurationOptions.Value;

            Capacity = _batteryConfiguration.Capacity;
            Charge = _batteryConfiguration.Charge;
            ChargingRate = _batteryConfiguration.ChargingRate;
            Efficiency = _batteryConfiguration.Efficiency;
            Status = BatteryStatus.Off;
            Temperature = _batteryConfiguration.Temperature;

            Console.WriteLine("Battery is created");
            Console.WriteLine($"Battery Capacity: {Capacity}");
            Console.WriteLine($"Battery Charge: {Charge}");
            Console.WriteLine($"Battery ChargingRate: {ChargingRate}");
            Console.WriteLine($"Battery Efficiency: {Efficiency}");
            Console.WriteLine($"Battery Status: {Status}");
            Console.WriteLine($"Battery Temperature: {Temperature}");
            Console.WriteLine($"Battery Percentage: {Percentage}");
        }

        // Methods
        public bool CheckBatteryStatus()
        {
            if (Percentage < 0.2)
            {
                Status = BatteryStatus.Warning;
                Console.WriteLine("Battery is low and in warning state");
            }
            if (Percentage < 0.01)
            {
                Status = BatteryStatus.Off;
                throw new Exception("Battery is empty and needs to be charged");
            }

            Console.WriteLine("Battery is OK");
            return true;
        }

        public void ChargeBattery()
        {
            Status = BatteryStatus.Charging;
            Charge += (ChargingRate * Efficiency);
            
            CheckCurrentBattery();

            if (Charge > Capacity)
            {
                Charge = Capacity;
                Status = BatteryStatus.Full;

                Console.WriteLine("Battery is full");
            }
        }

        public void DischargeBattery(double discharge)
        {
            Status = BatteryStatus.Discharging;

            double power = discharge / Efficiency;
            Charge -= (power);

            if (Charge < 0)
            {
                Charge = 0;
                Status = BatteryStatus.Off;
            }
            else if (Charge < 0.2 * Capacity)
            {
                Status = BatteryStatus.Warning;
            }
        }

        public void CheckCurrentBattery()
        {
            Console.WriteLine($"Battery Charge: {Charge}, Battery Status: {Status}");
            Console.WriteLine($"Battery Temperature: {Temperature}");
            Console.WriteLine($"Battery Percentage: {Percentage}");
        }

        public void UpdateTemperature(double temperature)
        {
            // TODO: Implement temperature
            this.Temperature = temperature;
        }
    }

    public enum BatteryStatus
    {
        Off,
        Charging,
        Discharging,
        Warning,
        Full
    }
}
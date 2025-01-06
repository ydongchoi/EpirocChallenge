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
        public double Power{ get; private set; }
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
        }

        // Methods
        public bool CheckBatteryStatus()
        {
            if (Percentage < 0.2)
            {
                Status = BatteryStatus.Warning;
                Console.WriteLine("Battery is low and in warning state\n");
            }
            if (Percentage < 0.01)
            {
                Status = BatteryStatus.Off;
                throw new Exception("Battery is empty and needs to be charged\n");
            }

            Console.WriteLine("Battery is OK\n");
            return true;
        }

        public void ChargeBattery()
        {
            UpdateTemperature(ChargingRate);

            Status = BatteryStatus.Charging;
            Power = - ChargingRate / Efficiency;
            Charge -= Power;
            
            if (Charge > Capacity)
            {
                Charge = Capacity;
                Status = BatteryStatus.Full;

                Console.WriteLine("Battery is full\n");
            }
        }

        public void DischargeBattery(double discharge, MotorStatus motorStatus)
        {
            if(motorStatus == MotorStatus.Off) Status = BatteryStatus.Off;
            else Status = BatteryStatus.Discharging;

            UpdateTemperature(discharge);

            Power = discharge / Efficiency;
            Charge -= (Power) * 0.1;

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
            Console.WriteLine($"Battery Charge: {Charge}, Battery Status: {Status}, Power: {Power}");
            Console.WriteLine($"Battery Temperature: {Temperature}");
            Console.WriteLine($"Battery Percentage: {Percentage * 100}%\n");
        }

        public void UpdateTemperature(double charge)
        {
            charge = Math.Abs(charge);
            
            double power = charge / Efficiency;
            double deltaPower = power - Power;

            bool isPowerPositive = deltaPower > 0;
 
            double energyLoss = Math.Pow(deltaPower, 2) * 0.05 * (isPowerPositive ? 1 : -1);
            double temperatureRise = energyLoss / (200000 * 1.5);

            if(charge > 1){
                Temperature += temperatureRise;
                Temperature = Math.Min(Temperature, 100);
            }
            else {
                Temperature = Math.Max(0, Temperature - 0.02);
            }
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
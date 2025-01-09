using Microsoft.Extensions.Options;
using MiningVehicle.Logger;
using MiningVehicle.VehicleEmulator.ConfigurationModels;

namespace MiningVehicle.VehicleEmulator.Components
{
    public class Battery
    {
        // Configuration
        private readonly BatteryConfiguration _batteryConfiguration;
        private readonly ILoggerManager _logger;

        // Properties
        public double Capacity { get; private set; }
        public double Charge { get; private set; }
        public double ChargingRate { get; private set; }
        public double Efficiency { get; private set; }
        public double Percentage => Charge / Capacity;
        public double Power { get; private set; }
        public BatteryStatus Status { get; private set; }
        public double Temperature { get; private set; }

        // Constructor
        public Battery(
            IOptions<BatteryConfiguration> batteryConfigurationOptions,
            ILoggerManager logger)
        {
            _batteryConfiguration = batteryConfigurationOptions.Value;
            _logger = logger;

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
                _logger.LogWarning("Battery is low and in warning state");
            }
            if (Percentage < 0.01)
            {
                PowerOff();
                _logger.LogWarning("Battery is empty and in off state");
            }

            _logger.LogInformation("Battery is OK");
            return true;
        }

        public void ChargeBattery()
        {
            UpdateTemperature(ChargingRate);

            Status = BatteryStatus.Charging;
            Power = -ChargingRate / Efficiency;
            Charge -= Power;

            if (Charge > Capacity)
            {
                Charge = Capacity;
                Status = BatteryStatus.Full;
                _logger.LogInformation("Battery is full");
            }
        }

        public void DischargeBattery(double discharge, MotorStatus motorStatus)
        {
            Status = motorStatus == MotorStatus.Off ? BatteryStatus.Off : BatteryStatus.Discharging;

            UpdateTemperature(discharge);

            Power = discharge / Efficiency;
            Charge -= Power * 0.1;

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
            _logger.LogInformation($"Battery Charge: {Charge}, Battery Status: {Status}, Power: {Power}");
            _logger.LogInformation($"Battery Temperature: {Temperature}");
            _logger.LogInformation($"Battery Percentage: {Percentage * 100}%\n");
        }

        private void UpdateTemperature(double charge)
        {
            if (Status == BatteryStatus.Off)
            {
                Temperature = 0;
                return;
            }

            charge = Math.Abs(charge);
            double power = charge / Efficiency;
            double deltaPower = power - Power;
            bool isPowerPositive = deltaPower > 0;

            double energyLoss = Math.Pow(deltaPower, 2) * 0.05 * (isPowerPositive ? 1 : -1);
            double temperatureRise = energyLoss / (200000 * 1.5);

            if (charge > 1)
            {
                Temperature += temperatureRise;
                Temperature = Math.Min(Temperature, 100);
            }
            else
            {
                Temperature = Math.Max(0, Temperature - 0.02);
            }
        }

        public void PowerOff()
        {
            Status = BatteryStatus.Off;
            Power = 0;
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

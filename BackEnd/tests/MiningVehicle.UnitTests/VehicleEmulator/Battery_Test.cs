using Microsoft.Extensions.Options;
using MiningVehicle.Logger;
using MiningVehicle.VehicleEmulator.Components;
using MiningVehicle.VehicleEmulator.ConfigurationModels;
using Xunit;

namespace MiningVehicle.UnitTests.VehicleEmulator
{
    public class Battery_Test
    {
        private readonly IOptions<BatteryConfiguration> _batteryConfigurationOptions;
        private readonly BatteryConfiguration _batteryConfiguration;
        private readonly ILoggerManager _logger;
        private readonly Battery _battery;

        private const int perSec = 10;


        public Battery_Test()
        {
            _batteryConfiguration = new BatteryConfiguration
            {
                Capacity = 675000,
                Charge = 675000,
                ChargingRate = 1000,
                Efficiency = 0.9,
                Temperature = 0
            };
            _batteryConfigurationOptions = Options.Create(_batteryConfiguration);
            
            _logger = new LoggerManager();

            _battery = new Battery(_batteryConfigurationOptions, _logger);
        }

        [Fact]
        public void Battery_InitialCharge_ShouldBeFull()
        {
            // Act
            var initialCharge = _battery.Charge;

            // Assert
            Assert.Equal(_batteryConfiguration.Charge, initialCharge);
        }

        [Fact]
        public void Battery_Discharge_ShouldReduceCharge()
        {
            // Arrange
            var efficiency = _batteryConfiguration.Efficiency;
            var initialCharge = _batteryConfiguration.Charge;
            var discharge = 10 / efficiency * 0.1;

            // Act
            _battery.DischargeBattery(10, MotorStatus.Running);

            // Assert
            Assert.Equal(initialCharge - discharge, _battery.Charge);
        }

        [Fact]
        public void Battery_Charge_ShouldIncreaseCharge()
        {
            // Arrange
            var chargingRate = _batteryConfiguration.ChargingRate;
            var efficiency = _batteryConfiguration.Efficiency;
       
            _battery.DischargeBattery(50000, MotorStatus.Running);
            var chargeAfterDischarge = _battery.Charge;

            // Act
            _battery.ChargeBattery();

            // Assert
            Assert.Equal(chargeAfterDischarge + chargingRate / efficiency, _battery.Charge);
        }

        [Fact]
        public void Battery_Discharge_ShouldNotGoBelowZero()
        {
            // Arrange
       
            // Act
            _battery.DischargeBattery(_batteryConfiguration.Charge * perSec + 1000, MotorStatus.Running);

            // Assert
            Assert.Equal(0, _battery.Charge);
        }

        [Fact]
        public void Battery_Charge_ShouldNotExceedFullCharge()
        {
            // Arrange
         
            // Act
            _battery.ChargeBattery();

            // Assert
            Assert.Equal(_batteryConfiguration.Charge, _battery.Charge);
        }

        [Fact]
        public void Battery_Discharge_ShouldHandlePartialDischarge()
        {
            // Arrange
            var efficiency = _batteryConfiguration.Efficiency;
            var initialCharge = _batteryConfiguration.Charge;
            var discharge = 30000 / efficiency * 0.1;

            // Act
            _battery.DischargeBattery(30000, MotorStatus.Running);

            // Assert
            Assert.Equal(initialCharge - discharge, _battery.Charge);
        }


        [Fact]
        public void Battery_CheckStatus_ShouldReturnWarningStatus()
        {
            // Arrange
            _battery.DischargeBattery(_batteryConfiguration.Charge * 0.8 * perSec, MotorStatus.Running);
            
            // Act
            var status = _battery.CheckBatteryStatus();

            // Assert
            Assert.True(_battery.Status == BatteryStatus.Warning);
        }

        [Fact]
        public void Battery_CheckStatus_ShouldReturnOffStatus()
        {
            // Arrange
            _battery.DischargeBattery(_batteryConfiguration.Charge * 0.99 * perSec, MotorStatus.Running);
            
            // Act
            var status = _battery.CheckBatteryStatus();

            // Assert
            Assert.True(_battery.Status == BatteryStatus.Off);
        }
    }
}
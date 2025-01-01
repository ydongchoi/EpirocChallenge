using Microsoft.Extensions.Options;
using MiningVehicle.VehicleEmulator.Components;
using MiningVehicle.VehicleEmulator.ConfigurationModels;
using Xunit;

namespace MiningVehicle.UnitTests.VehicleEmulator
{
    public class Battery_Test
    {
        private readonly IOptions<BatteryConfiguration> _batteryConfigurationOptions;
        
        private readonly Battery _battery;

        public Battery_Test(IOptions<BatteryConfiguration> batteryConfigurationOptions)
        {
            _batteryConfigurationOptions = batteryConfigurationOptions;
            _battery = new Battery(_batteryConfigurationOptions);
        }

        [Fact]
        public void Battery_InitialCharge_ShouldBeFull()
        {
            // Act
            var initialCharge = _battery.Charge;

            // Assert
            Assert.Equal(6000, initialCharge);
        }

        [Fact]
        public void Battery_Discharge_ShouldReduceCharge()
        {
            // Arrange
            var efficiency = 0.9;
            var initialCharge = _battery.Charge;

            // Act
            _battery.DischargeBattery(10);

            // Assert
            Assert.Equal(initialCharge - 10 / efficiency, _battery.Charge);
        }

        [Fact]
        public void Battery_Charge_ShouldIncreaseCharge()
        {
            // Arrange
            var chargingRate = 2.77;
            var efficiency = 0.9;
       
            _battery.DischargeBattery(50);
            var chargeAfterDischarge = _battery.Charge;

            // Act
            _battery.ChargeBattery();

            // Assert
            Assert.Equal(chargeAfterDischarge + chargingRate * efficiency, _battery.Charge);
        }

        [Fact]
        public void Battery_Discharge_ShouldNotGoBelowZero()
        {
            // Arrange
       
            // Act
            _battery.DischargeBattery(6500);

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
            Assert.Equal(6000, _battery.Charge);
        }
    }
}
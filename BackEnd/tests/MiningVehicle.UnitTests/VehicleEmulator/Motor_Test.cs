using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using MiningVehicle.VehicleEmulator.ConfigurationModels;
using MiningVehicle.VehicleEmulator.Components;

namespace MiningVehicle.UnitTests.VehicleEmulator
{
    public class Motor_Test
    {
        private readonly IOptions<BatteryConfiguration> _batteryConfigurationOptions;
        private readonly IOptions<MotorConfiguration> _motorConfigurationOptions;

        private readonly Mock<Battery> _mockBattery;
        private readonly Motor _motor;

        public Motor_Test()
        {
            _mockBattery = new Mock<Battery>(_batteryConfigurationOptions);
            _motor = new Motor(_motorConfigurationOptions, _mockBattery.Object);
        }
        
        [Fact]
        public void StartMotor_ShouldSetIsRunningToTrue()
        {
            // Act
            _motor.StartMotor();

            // Assert
            Assert.True(_motor.Status == MotorStatus.Idle);
        }

        [Fact]
        public void StopMotor_ShouldSetIsRunningToFalse()
        {
            // Arrange
            _motor.StartMotor();

            // Act
            _motor.StopMotor();

            // Assert
            Assert.False(_motor.Status == MotorStatus.Running);
        }
    }
}
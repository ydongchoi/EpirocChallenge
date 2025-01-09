using Microsoft.Extensions.Options;
using MiningVehicle.Logger;
using MiningVehicle.VehicleEmulator.ConfigurationModels;

namespace MiningVehicle.VehicleEmulator.Components
{
    public class Motor
    {
        // Configuration
        private readonly MotorConfiguration _motorConfiguration;
        private readonly ILoggerManager _logger;

        // Dependencies
        private readonly Battery _battery;

        // Properties
        public double GearRatio { get; private set; } // 1:6 (motor:wheel)
        public int NominalPower { get; private set; }
        public int NominalTorque { get; private set; }
        public double Power { get; private set; }
        public int Rpm { get; private set; }
        public MotorStatus Status { get; private set; }
        private int speed;
        public int Speed
        {
            get => speed;
            set
            {
                if (value < 0 || value > 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(Speed), "Speed must be between 0 and 4.");
                }
                speed = value;
            }
        }

        public Motor(
            IOptions<MotorConfiguration> motorConfigurationOption,
            ILoggerManager logger,
            Battery battery)
        {
            _motorConfiguration = motorConfigurationOption.Value;
            _logger = logger;
            _battery = battery;

            NominalPower = _motorConfiguration.NominalPower;
            NominalTorque = _motorConfiguration.NominalTorque;
            GearRatio = (double)_motorConfiguration.MotorRotation / _motorConfiguration.WheelRotation;
            Rpm = 0;
            Status = MotorStatus.Off;
        }

        public bool CheckMotorStatus(int rpm)
        {
            if (rpm < 0 || rpm > 800)
            {
                Status = MotorStatus.Fault;
                _logger.LogError("Motor is in fault state");

                return false;
            }
            else if (rpm > 600)
            {
                WarnMotor();
                
                return true;
            }
            else
            {
                _logger.LogInformation("Motor is OK");
                
                return true;
            }
        }

        public void StartMotor()
        {
            Status = MotorStatus.Idle;
            _logger.LogInformation("Starting motor...");
        }

        public void AdjustSpeed(int speed)
        {
            if (speed == -1)
            {
                StopMotor();
                return;
            }

            Status = speed switch
            {
                0 => MotorStatus.Idle,
                _ => MotorStatus.Running
            };

            Speed = speed;

            int targetRpm = CalculateTargetRpm(speed);
            Rpm = (int)Lerp(Rpm, targetRpm, 0.1);

            double power = CalculatePower();
            _battery.DischargeBattery(power, Status);

            if (_battery.Status == BatteryStatus.Off)
            {
                StopMotor();
            }
        }

        private int CalculateTargetRpm(int speed)
        {
            return 100 * (int)Math.Pow(2, speed - 1);
        }

        private double CalculatePower()
        {
            if (Rpm == 0)
            {
                return 0;
            }

            double angularVelocity = (2 * Math.PI * Rpm) / 60;
            Power = angularVelocity * NominalTorque;

            return Power;
        }

        private double Lerp(double current, double target, double t)
        {
            return current + (target - current) * t * 3;
        }

        public void StopMotor()
        {
            Rpm = 0;
            Status = MotorStatus.Off;
            _logger.LogInformation("Motor stopped");
        }

        private void WarnMotor()
        {
            Status = MotorStatus.Warning;
            _logger.LogWarning("Motor is in warning state");
        }
    }

    public enum MotorStatus
    {
        Off,
        Idle,
        Running,
        Warning,
        Fault
    }
}

using Microsoft.Extensions.Options;
using MiningVehicle.VehicleEmulator.ConfigurationModels;

namespace MiningVehicle.VehicleEmulator.Components
{
    public class Motor
    {
        // Configuration
        private readonly IOptions<MotorConfiguration> _motorConfigurationOptions;
        private readonly MotorConfiguration _motorConfiguration;

        // Dependencies
        private readonly Battery _battery;

        // Properties
        public double GearRatio { get; private set; } // 1:6 (motor:wheel)
        public int Rpm { get; private set; } // 150 kW (about 200 horsepower)
        public MotorStatus Status { get; private set; }
        private int speed;
        public int Speed // 0 -> 1 100rpm 10kw -> 2 200rpm 20kw -> 3 400rpm 30kw -> 4 800rpm 40kw per hour.
        {
            get { return speed; }
            set
            {
                if (value < 0 || value > 4)
                {
                    throw new ArgumentOutOfRangeException(nameof(Speed), "Speed must be between 0 and 4.");
                }
                speed = value;
            }
        }

        public Motor(IOptions<MotorConfiguration> motorConfigurationOption, Battery battery)
        {
            _motorConfigurationOptions = motorConfigurationOption;
            _motorConfiguration = _motorConfigurationOptions.Value;

            _battery = battery;

            GearRatio = (double)_motorConfiguration.MotorRotation / _motorConfiguration.WheelRotation;
            Rpm = _motorConfiguration.Rpm;
            Status = MotorStatus.Off;

            Console.WriteLine("Motor is created");
            Console.WriteLine($"Motor Gear Ratio: {GearRatio}");
            Console.WriteLine($"Motor RPM: {Rpm}");
            Console.WriteLine($"Motor Status: {Status}");
        }

        public void CheckMotorStatus()
        {
            if (Status == MotorStatus.Fault)
            {
                throw new Exception("Motor is in fault state");
            }

            Console.WriteLine("Motor is OK");
        }

        public void StartMotor()
        {
            Console.WriteLine("Starting motor...");
            Status = MotorStatus.Idle;
        }

        public void AdjustSpeed(int speed)
        {
            Status = MotorStatus.Running;
            this.speed = speed;

            int targetRpm = CalculateTargetRpm(speed);
            Rpm = (int)Lerp(Rpm, targetRpm, 0.1);

            double power = CalculatePower(speed);
            _battery.DischargeBattery(power);

            Console.WriteLine($"Motor RPM: {Rpm}, Target RPM: {targetRpm} Power: {power}");
        }

        /// <summary>
        /// Calculates the target RPM (Revolutions Per Minute) based on the given speed.
        /// </summary>
        /// <param name="speed">The speed at which the vehicle is moving.</param>
        /// <returns>The calculated target RPM.</returns>
        private int CalculateTargetRpm(int speed)
        {
            return 100 * (int)Math.Pow(2, speed - 1);
        }

        /// <summary>
        /// Calculates the power required to maintain the given speed.
        /// <paramref name="speed"/>The speed at which the vehicle is moving.</param>
        /// </summary>
        private double CalculatePower(int speed)
        {
            double angularVelocity = (2 * Math.PI * Rpm) / 60;
            double power = Lerp(0, 10000 * speed, 0.1);
            double torque = power / angularVelocity;

            return Rpm * torque * (0.1 / 3600);
        }

        private double Lerp(double current, double target, double t)
        {
            double delta = target - current;
            current = current + delta * t * 3;

            return current;
        }

        public void StopMotor()
        {
            Console.WriteLine("Stopping motor...");
            Status = MotorStatus.Off;
        }

        private void WarnMotor()
        {
            Console.WriteLine("Motor is in warning state");
            Status = MotorStatus.Warning;
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
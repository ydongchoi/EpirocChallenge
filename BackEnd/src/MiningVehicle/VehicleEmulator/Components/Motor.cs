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
        public int NominalPower { get; private set; } 
        public int NominalTorque { get; private set; } 
        public double Power { get; private set; }
        public int Rpm { get; private set; }
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

            NominalPower = _motorConfiguration.NominalPower;
            NominalTorque = _motorConfiguration.NominalTorque;
            GearRatio = (double)_motorConfiguration.MotorRotation / _motorConfiguration.WheelRotation;
            Rpm = 0;
            Status = MotorStatus.Off;
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
            Console.WriteLine("Starting motor...\n");
            Status = MotorStatus.Idle;
        }

        public void AdjustSpeed(int speed)
        {
            Status = MotorStatus.Running;
            this.speed = speed;

            int targetRpm = CalculateTargetRpm(speed);
            Rpm = (int)Lerp(Rpm, targetRpm, 0.1);

            double power = CalculatePower();
            _battery.DischargeBattery(power);

            if(_battery.Status == BatteryStatus.Off){
                StopMotor();
                return;
            }
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
        /// </summary>
        private double CalculatePower()
        {
            if(Rpm == 0)
            {
                return 0;
            }

            double angularVelocity = (2 * Math.PI * Rpm) / 60;
            Power = angularVelocity * NominalTorque;

            return Power;
        }

        private double Lerp(double current, double target, double t)
        {
            double delta = target - current;
            current = current + delta * t * 3;

            return current;
        }

        public void StopMotor()
        {
            Console.WriteLine("Stopping motor...\n");
            Status = MotorStatus.Off;
        }

        private void WarnMotor()
        {
            Console.WriteLine("Motor is in warning state\n");
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
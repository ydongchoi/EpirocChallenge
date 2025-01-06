using System.Timers;
using MiningVehicle.VehicleEmulator.Components;
using MiningVehicle.VehicleEmulator.Models;

namespace MiningVehicle.VehicleEmulator
{
    public sealed class MiningVehicleEmulator : IMiningVehicleEmulator, IDisposable
    {
        // Components
        private readonly Battery _battery;
        private readonly Motor _motor;

        // SignalR
        private readonly IMiningVehicleClient _miningVehicleClient;

        private int _speed;
        private System.Timers.Timer _timer;

        public MiningVehicleEmulator(Battery battery, Motor motor, IMiningVehicleClient miningVehicleClient)
        {
            _battery = battery ?? throw new ArgumentNullException(nameof(battery));
            _motor = motor ?? throw new ArgumentNullException(nameof(motor));
            _miningVehicleClient = miningVehicleClient ?? throw new ArgumentNullException(nameof(miningVehicleClient));

            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += OnTimedEvent;
        }

        public void StartEngine()
        {
            _timer.Start();
            _miningVehicleClient.ConnectAsync().Wait();

            Console.WriteLine("Checking battery status...");
            _battery.CheckBatteryStatus();

            Console.WriteLine("Checking motor status...");
            _motor.CheckMotorStatus();

            Console.WriteLine("Condition check passed, starting the engine...\n");
            _motor.StartMotor();

            SendVehicleDataAsync().Wait();
        }

        public void StopEngine()
        {
            Console.WriteLine("Stopping the engine...\n");
            _motor.StopMotor();
            _battery.PowerOff();
        }

        public async Task AdjustSpeed(int speed)
        {
            if (_motor.Status == MotorStatus.Off) StartEngine();

            Console.WriteLine($"Adjusting speed to {speed}...\n");
            _speed = speed;
            _motor.AdjustSpeed(speed);
            _battery.CheckCurrentBattery();

            await SendVehicleDataAsync();
        }

        public void Break()
        {
            Console.WriteLine("Breaking...\n");
            _motor.StopMotor();
        }

        public async Task ChargeBattery()
        {
            Console.WriteLine("Charging battery...\n");
            _battery.ChargeBattery();

            await SendVehicleDataAsync();
        }

        public async Task StopBatteryCharging()
        {
            Console.WriteLine("Stopping battery charging...\n");
            _battery.PowerOff();

            await SendVehicleDataAsync();
        }

        private VehicleData GetVehicleData()
        {
            return new VehicleData
            {
                Timestamp = DateTime.Now,
                BatteryData = new BatteryData
                {
                    Capacity = _battery.Capacity,
                    Charge = _battery.Charge,
                    ChargingRate = _battery.ChargingRate,
                    Efficiency = _battery.Efficiency,
                    Status = _battery.Status,
                    Percentage = _battery.Percentage,
                    Power = _battery.Power,
                    Temperature = _battery.Temperature
                },
                BreakData = new BreakData
                {
                    Status = BreakStatus.Off
                },
                MotorData = new MotorData
                {
                    GearRatio = _motor.GearRatio,
                    Status = _motor.Status,
                    Speed = _motor.Speed,
                    Rpm = _motor.Rpm
                }
            };
        }

        private async Task SendVehicleDataAsync()
        {
            var vehicleData = GetVehicleData();
            await _miningVehicleClient.SendVehicleDataAsync(vehicleData);
        }

        private void OnTimedEvent(object? source, ElapsedEventArgs e)
        {
            _miningVehicleClient.ConnectAsync();

            if (_battery.Status == BatteryStatus.Charging)
            {
                ChargeBattery().Wait();
            }

            if (_motor.Status == MotorStatus.Idle || _motor.Status == MotorStatus.Running)
            {
                AdjustSpeed(_speed).Wait();
            }
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
            _miningVehicleClient.DisconnectAsync().Wait();
        }
    }
}

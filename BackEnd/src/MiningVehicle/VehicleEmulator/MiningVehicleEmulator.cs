using System.Timers;
using MiningVehicle.VehicleEmulator.Components;
using MiningVehicle.VehicleEmulator.Models;

namespace MiningVehicle.VehicleEmulator
{
    public sealed class MinigVehicleEmulator : IMiningVehicleEmulator, IDisposable
    {

        // Components
        private Battery _battery;
        private Motor _motor;

        // SignalR
        private IMiningVehicleClient _miningVehicleClient;

        private int _speed;
        private System.Timers.Timer _timer;

        public MinigVehicleEmulator(Battery battery, Motor motor, IMiningVehicleClient miningVehicleClient)
        {
            _battery = battery;
            _motor = motor;

            _miningVehicleClient = miningVehicleClient;
            _miningVehicleClient.ConnectAsync();
        
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += OnTimedEvent;
        }

        public void StartEngine()
        {
            _timer.Start();

            Console.WriteLine("Checking battery status...");
            _battery.CheckBatteryStatus();

            Console.WriteLine("Checking motor status...");
            _motor.CheckMotorStatus();

            Console.WriteLine("Condition check passed, starting the engine...\n");
            _motor.StartMotor();

            var vehicleData = GetVehicleData();
            _miningVehicleClient.SendVehicleDataAsync(vehicleData).Wait();
        }

        public void StopEngine()
        {
            Console.WriteLine("Stopping the engine...\n");
            _motor.StopMotor();
        }

        public async Task AdjustSpeed(int speed)
        {
            if(_motor.Status == MotorStatus.Off) StartEngine();

            Console.WriteLine($"Adjusting speed to {speed}...\n");
            _speed = speed;
            _motor.AdjustSpeed(speed);
            _battery.CheckCurrentBattery();

            var vehicleData = GetVehicleData();
            await _miningVehicleClient.SendVehicleDataAsync(vehicleData);
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

            var vehicleData = GetVehicleData();
            await _miningVehicleClient.SendVehicleDataAsync(vehicleData);
        }

        private VehicleData GetVehicleData()
        {

            var batteryData = new BatteryData
            {
                Capacity = _battery.Capacity,
                Charge = _battery.Charge,
                ChargingRate = _battery.ChargingRate,
                Efficiency = _battery.Efficiency,
                Status = _battery.Status,
                Percentage = _battery.Percentage,
                Power = _battery.Power,
                Temperature = _battery.Temperature
            };

            var motorData = new MotorData
            {
                GearRatio = _motor.GearRatio,
                Status = _motor.Status,
                Speed = _motor.Speed,
                Rpm = _motor.Rpm
            };


            var vehicleData = new VehicleData
            {
                Timestamp = DateTime.Now,
                BatteryData = batteryData,
                MotorData = motorData
            };

            return vehicleData;
        }

        private void OnTimedEvent(object? source, ElapsedEventArgs e)
        {
            _miningVehicleClient.ConnectAsync();

            if (_battery.Status == BatteryStatus.Charging)
            {   
                ChargeBattery();
            }

            if (_motor.Status == MotorStatus.Idle || _motor.Status == MotorStatus.Running)
            {
                AdjustSpeed(_speed).Wait();
            }
        }

        public void Dispose()
        {
            // TODO : Implement IDisposable to Motor and Battery
            _timer.Stop();
            _timer.Dispose();
            _miningVehicleClient.DisConnectAsync().Wait();
        }
    }
}
using System.Timers;
using MiningVehicle.Logger;
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
        private readonly ILoggerManager _logger;

        private int _speed;
        private System.Timers.Timer _timer;

        public MiningVehicleEmulator(
            Battery battery,
            Motor motor,
            IMiningVehicleClient miningVehicleClient,
            ILoggerManager logger)
        {
            _battery = battery ?? throw new ArgumentNullException(nameof(battery));
            _motor = motor ?? throw new ArgumentNullException(nameof(motor));

            _miningVehicleClient = miningVehicleClient ?? throw new ArgumentNullException(nameof(miningVehicleClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += OnTimedEvent;
        }

        public async Task StartEngineAsync()
        {
            _timer.Start();
            
            _miningVehicleClient.ConnectAsync().Wait();

            bool isVehicleConditonOk = CheckMotorAndBatteryStatus();
            if (!isVehicleConditonOk)
            {
                _logger.LogError("Vehicle condition is not OK. Engine cannot be started.");
                await StopEngineAsync();

                return;
            }

            _logger.LogInformation("Starting the engine...");
            _motor.StartMotor();

            SendVehicleDataAsync().Wait();
        }

        public async Task StopEngineAsync()
        {
            _logger.LogInformation("Stopping the engine...");
            _motor.StopMotor();
            _battery.PowerOff();

            await SendVehicleDataAsync();
        }

        public async Task AdjustSpeedAsync(int speed)
        {
            if (_motor.Status == MotorStatus.Off) await StartEngineAsync();

            bool isVehicleConditonOk = CheckMotorAndBatteryStatus();   
            if (!isVehicleConditonOk)
            {
                _logger.LogError("Vehicle condition is not OK. Engine cannot be started.");

                await StopEngineAsync();
                return;
            }

            _logger.LogInformation($"Adjusting speed to {speed}...");

            _speed = speed;
            _motor.AdjustSpeed(speed);

            await SendVehicleDataAsync();
        }

        public async Task BreakAsync()
        {
            _logger.LogInformation("Breaking...");
            _motor.StopMotor();
        }

        public async Task ChargeBatteryAsync()
        {
            _logger.LogInformation("Charging battery...");
            StopEngineAsync().Wait();
            _battery.ChargeBattery();

            await SendVehicleDataAsync();
        }

        public bool CheckMotorAndBatteryStatus()
        {
            _logger.LogInformation("Checking motor and battery status...");

            bool isBatteryOk = _battery.CheckBatteryStatus();
            bool isMotorOk = _motor.CheckMotorStatus(_motor.Rpm);

            if (isBatteryOk && isMotorOk)
            {
                _logger.LogInformation("Motor and battery are OK");
                return true;
            }

            if (!isBatteryOk)
            {
                _logger.LogError("Battery is in fault state");
            }
            if (!isMotorOk)
            {
                _logger.LogError("Motor is in fault state");
            }

            return false;
        }

        public async Task StopBatteryChargingAsync()
        {
            _logger.LogInformation("Stopping battery charging...");
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
            _miningVehicleClient.ConnectAsync().Wait();

            if (_battery.Status == BatteryStatus.Charging)
            {
                ChargeBatteryAsync().Wait();
            }
            else if (_motor.Status == MotorStatus.Idle || _motor.Status == MotorStatus.Running || _motor.Status == MotorStatus.Warning)
            {
                AdjustSpeedAsync(_speed).Wait();
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

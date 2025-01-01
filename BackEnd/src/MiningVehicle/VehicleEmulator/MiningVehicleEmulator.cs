using System.Timers;
using MiningVehicle.VehicleEmulator.Components;

namespace MiningVehicle.VehicleEmulator{
    public sealed class MinigVehicleEmulator: IMiningVehicleEmulator, IDisposable{
    
    // Components
    private Battery _battery;
    private Motor _motor;
    private int _speed;
    private System.Timers.Timer _timer;

    public MinigVehicleEmulator(Battery battery, Motor motor){
        _battery = battery;        
        _motor = motor;

        _timer = new System.Timers.Timer(100);
        _timer.Elapsed += OnTimedEvent;
    }

    public void StartEngine(){
        Console.WriteLine("Checking battery status...");
        _battery.CheckBatteryStatus();

        Console.WriteLine("Checking motor status...");
        _motor.CheckMotorStatus();

        Console.WriteLine("Condition check passed, starting the engine...");
        _motor.StartMotor();        
        
        _timer.Start();
    }    

    public void StopEngine(){
        Console.WriteLine("Stopping the engine...");
        _motor.StopMotor();
    }

    public void AdjustSpeed(int speed){
        Console.WriteLine($"Adjusting speed to {speed}...");
        _speed = speed;
        _motor.AdjustSpeed(speed);
        _battery.CheckCurrentBattery();
    }

    public void Break(){
        Console.WriteLine("Breaking...");
        _motor.StopMotor();
    }

    public void ChargeBattery(){
        Console.WriteLine("Charging battery...");
        _battery.ChargeBattery();
    }

    private void OnTimedEvent(object? source, ElapsedEventArgs e)
    {
        if(_battery.Status == BatteryStatus.Charging){
            ChargeBattery();
        }

        if(_motor.Status == MotorStatus.Idle || _motor.Status == MotorStatus.Running){
            AdjustSpeed(_speed);
        }
    }

    public void Dispose(){
        // TODO : Implement IDisposable to Motor and Battery
        _timer.Stop();
        _timer.Dispose();
    }
}   
}
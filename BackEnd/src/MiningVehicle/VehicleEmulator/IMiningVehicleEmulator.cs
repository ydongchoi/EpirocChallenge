namespace MiningVehicle.VehicleEmulator
{
    public interface IMiningVehicleEmulator{
        public void StartEngine();
        public void StopEngine();

        public void AdjustSpeed(int speed);
        public void Break();
        
        public void ChargeBattery();
    }
}
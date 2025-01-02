namespace MiningVehicle.API.DTO
{
    public class VehicleStatusDTO
    {
        public int Speed { get; set; }
        public int BatteryLevel { get; set; }
        public bool EngineRunning { get; set; }
    }
}
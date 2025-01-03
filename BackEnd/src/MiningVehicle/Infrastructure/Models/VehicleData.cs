using MiningVehicle.VehicleEmulator.Components;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiningVehicle.Infrastructure.Models
{
    public class VehicleData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("timestamp")]
        public DateTime TimeStamp { get; set; }

        [BsonElement("battery")]
        public BatteryData BatteryData { get; set; }

        [BsonElement("motor")]
        public MotorData MotorData { get; set; }

        [BsonElement("brake")]
        public BrakeData BrakeData { get; set; }
    }

    public class BatteryData
    {
        [BsonElement("status")]
        public BatteryStatus Status { get; set; }

        [BsonElement("capacity")]
        public double Capacity { get; set; }

        [BsonElement("charge")]
        public double Charge { get; set; }

        [BsonElement("chargingRate")]
        public double ChargingRate { get; set; }

        [BsonElement("efficiency")]
        public double Efficiency { get; set; }
        
        [BsonElement("percentage")]
        public double Percentage { get; set; }
        
        [BsonElement("power")]
        public double Power { get; set; }

        [BsonElement("temperature")]
        public double Temperature { get; set; }
    }

    public class BrakeData
    {
        [BsonElement("status")]
        public BreakStatus Status { get; set; }
    }

    public class MotorData
    {
        [BsonElement("status")]
        public MotorStatus Status { get; set; }

        [BsonElement("speed")]
        public double Speed { get; set; }

        [BsonElement("rpm")]
        public double Rpm { get; set; }

        [BsonElement("gearRatio")]
        public double GearRatio { get; set; }
    }
}
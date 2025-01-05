using MiningVehicle.Infrastructure.Data;
using MiningVehicle.Infrastructure.Models;
using MongoDB.Driver;

namespace MiningVehicle.Infrastructure.Repositories
{
    public class VehicleDataRepository : IRepository
    {
        private readonly IMongoCollection<VehicleData> _vehicleDataCollection;

        public VehicleDataRepository(MongoDbContext mongoDbContext)
        {
            Console.WriteLine("VehicleDataRepository constructor");
            _vehicleDataCollection = mongoDbContext.GetCollection<VehicleData>("VehicleData");
            Console.WriteLine(_vehicleDataCollection);
        }

        public async Task AddVehicleDataAsync(VehicleData vehicleData)
        {
            Console.WriteLine("Adding vehicle data to database...");
            await _vehicleDataCollection.InsertOneAsync(vehicleData);
            Console.WriteLine("Vehicle data added to database.");
        }
    }
}
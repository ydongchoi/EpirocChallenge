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
            _vehicleDataCollection = mongoDbContext.GetCollection<VehicleData>("VehicleData");
        }

        public async Task AddVehicleDataAsync(VehicleData vehicleData)
        {
            await _vehicleDataCollection.InsertOneAsync(vehicleData);
        }
    }
}
using MiningVehicle.Infrastructure.Data;
using MiningVehicle.Infrastructure.Models;
using MiningVehicle.Logger;
using MongoDB.Driver;

namespace MiningVehicle.Infrastructure.Repositories
{
    public class VehicleDataRepository : IRepository
    {
        private readonly IMongoCollection<VehicleData> _vehicleDataCollection;
        private readonly ILoggerManager _logger;

        public VehicleDataRepository(
            MongoDbContext mongoDbContext,
            ILoggerManager logger)
        {
            _vehicleDataCollection = mongoDbContext.GetCollection<VehicleData>("VehicleData");
            _logger = logger;
         
            logger.LogInformation($"VehicleDataCollection: {_vehicleDataCollection.CollectionNamespace.CollectionName}");
        }

        public async Task AddVehicleDataAsync(List<VehicleData> vehicleDataList)
        {
            var bulkOps = new List<WriteModel<VehicleData>>();

            foreach (var vehicleData in vehicleDataList)
            {
                var insertOne = new InsertOneModel<VehicleData>(vehicleData);
                bulkOps.Add(insertOne);
            }

            await _vehicleDataCollection.BulkWriteAsync(bulkOps);          
            _logger.LogInformation("Added vehicle data to database...");
        }
    }
}
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

        public async Task AddVehicleDataAsync(List<VehicleData> vehicleDataList)
        {
            var bulkOps = new List<WriteModel<VehicleData>>();

            foreach (var vehicleData in vehicleDataList)
            {
                var insertOne = new InsertOneModel<VehicleData>(vehicleData);
                bulkOps.Add(insertOne);
            }

            Console.WriteLine($"Adding vehicle data to database...");
            await _vehicleDataCollection.BulkWriteAsync(bulkOps);
            Console.WriteLine("Saved to database...");
        }
    }
}
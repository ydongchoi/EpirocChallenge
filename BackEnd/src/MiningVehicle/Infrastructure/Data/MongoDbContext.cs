using Microsoft.Extensions.Options;
using MiningVehicle.Infrastructure.ConfigurationModels;
using MongoDB.Driver;

namespace MiningVehicle.Infrastructure.Data
{
    public class MongoDbContext
    {
        private readonly IOptions<MongoDbConfiguration> _mongoDbCongigurationOptions;
        private readonly MongoDbConfiguration _mongoDbConfiguration;
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbConfiguration> mongoDBConfigurationOptions)
        {
            _mongoDbCongigurationOptions = mongoDBConfigurationOptions;
            _mongoDbConfiguration = _mongoDbCongigurationOptions.Value;

            var client = new MongoClient(_mongoDbConfiguration.ConnectionString);
            _database = client.GetDatabase(_mongoDbConfiguration.Database);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
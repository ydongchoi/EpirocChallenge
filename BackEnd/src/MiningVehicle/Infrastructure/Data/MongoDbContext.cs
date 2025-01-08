using System.Security.Authentication;
using Microsoft.Extensions.Options;
using MiningVehicle.Infrastructure.ConfigurationModels;
using MiningVehicle.Logger;
using MongoDB.Driver;

namespace MiningVehicle.Infrastructure.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILoggerManager _logger;

        public MongoDbContext(
            IOptions<MongoDbConfiguration> mongoDBConfigurationOptions,
            ILoggerManager logger)
        {
            var mongoDbConfiguration = mongoDBConfigurationOptions.Value;
            _logger = logger;
            
            var connectionString = Environment.GetEnvironmentVariable("MongoDbConfiguration_ConnectionString") ?? mongoDbConfiguration.ConnectionString;
            var databaseName = Environment.GetEnvironmentVariable("MongoDbConfiguration_Database") ?? mongoDbConfiguration.Database;

            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            
            var client = new MongoClient(settings);
            _database = client.GetDatabase(databaseName);

            _logger.LogInformation($"MongoDB Connection String: {connectionString}");
            _logger.LogInformation($"MongoDB Database Name: {databaseName}");
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}

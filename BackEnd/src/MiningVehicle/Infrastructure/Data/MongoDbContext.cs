using System.Security.Authentication;
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
            var connectionString = Environment.GetEnvironmentVariable("MongoDbConfiguration_ConnectionString");
            var databaseName = Environment.GetEnvironmentVariable("MongoDbConfiguration_Database");

            Console.WriteLine($"MongoDB Connection String: {connectionString}");
            Console.WriteLine($"MongoDB Database Name: {databaseName}");

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);

            //var client = new MongoClient(_mongoDbConfiguration.ConnectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
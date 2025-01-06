using System.Security.Authentication;
using Microsoft.Extensions.Options;
using MiningVehicle.Infrastructure.ConfigurationModels;
using MongoDB.Driver;

namespace MiningVehicle.Infrastructure.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbConfiguration> mongoDBConfigurationOptions)
        {
            var mongoDbConfiguration = mongoDBConfigurationOptions.Value;
            var connectionString = Environment.GetEnvironmentVariable("MongoDbConfiguration_ConnectionString") ?? mongoDbConfiguration.ConnectionString;
            var databaseName = Environment.GetEnvironmentVariable("MongoDbConfiguration_Database") ?? mongoDbConfiguration.Database;

            Console.WriteLine($"MongoDB Connection String: {connectionString}");
            Console.WriteLine($"MongoDB Database Name: {databaseName}");

            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);

            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}

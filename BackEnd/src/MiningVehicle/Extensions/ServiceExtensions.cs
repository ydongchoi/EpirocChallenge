using Microsoft.AspNetCore.SignalR.Client;
using MiningVehicle.Infrastructure.ConfigurationModels;
using MiningVehicle.VehicleEmulator;
using MiningVehicle.Infrastructure.Data;
using MiningVehicle.VehicleEmulator.Components;
using MiningVehicle.VehicleEmulator.ConfigurationModels;

namespace MiningVehicle.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var reactAppUrl = environment == "Development"
                        ? "http://localhost:5173"
                        : "https://happy-river-0f3b0221e.4.azurestaticapps.net";

                    builder.WithOrigins("https://happy-river-0f3b0221e.4.azurestaticapps.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

        public static void AddAzureSignalRHub(this IServiceCollection services, IConfiguration configuration)
        {
            var azureSignalrConnectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_SIGNALR");
            Console.WriteLine($"Azure SignalR Connection String: {azureSignalrConnectionString.ToString()}");

            services.AddSignalR().AddAzureSignalR(opt =>
                opt.ConnectionString = azureSignalrConnectionString
            );
        }

        public static IServiceCollection AddSignalRClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<HubConnection>(provider =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var hubUrl = environment == "Development"
                    ? "http://localhost:5140/vehicleDataHub"
                    : "https://mining-vehicle.azurewebsites.net/vehicleDataHub";

                Console.WriteLine($"Hub URL: {hubUrl}");

                var hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://mining-vehicle.azurewebsites.net/vehicleDataHub")
                    .WithAutomaticReconnect()
                    .Build();

                Console.WriteLine("Hub Connection:");
                Console.WriteLine(hubConnection.ToString());

                return hubConnection;
            });

            return services;
        }

        public static void AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDbConfiguration"));
            services.AddSingleton<MongoDbContext>();
        }

        public static void AddMiningVehicle(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BatteryConfiguration>(configuration.GetSection("BatteryConfiguration"));
            services.Configure<MotorConfiguration>(configuration.GetSection("MotorConfiguration"));

            services.AddSingleton<Battery>();
            services.AddSingleton<Motor>();
            services.AddSingleton<IMiningVehicleEmulator, MinigVehicleEmulator>();
        }
    }
}
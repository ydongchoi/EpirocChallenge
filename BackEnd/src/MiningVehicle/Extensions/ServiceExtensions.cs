using Microsoft.AspNetCore.SignalR.Client;
using MiningVehicle.Infrastructure.ConfigurationModels;
using MiningVehicle.VehicleEmulator;
using MiningVehicle.Infrastructure.Data;
using MiningVehicle.VehicleEmulator.Components;
using MiningVehicle.VehicleEmulator.ConfigurationModels;
using Microsoft.AspNetCore.Http.Connections;
using MiningVehicle.Logger;

namespace MiningVehicle.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed((host) => true);
                });
            });

        public static void AddAzureSignalRHub(this IServiceCollection services, IConfiguration configuration)
        {
            var azureSignalrConnectionString = Environment.GetEnvironmentVariable("SignalR_ConnectionString");

            services
                .AddSignalR(opt => opt.KeepAliveInterval = TimeSpan.FromSeconds(60))
                .AddAzureSignalR(opt =>
                {
                    opt.ConnectionString = azureSignalrConnectionString;
                    opt.MaxHubServerConnectionCount = 10;
                });
        }

        public static IServiceCollection AddSignalRClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<HubConnection>(provider =>
            {
                var hubUrl = GetHubUrl();
                Console.WriteLine($"Hub URL: {hubUrl}");

                var hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl, options =>
                    {
                        options.Transports = HttpTransportType.WebSockets;
                        options.SkipNegotiation = false;
                    })
                    .WithStatefulReconnect()
                    .Build();

                hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(300);
                hubConnection.ServerTimeout = TimeSpan.FromSeconds(300);

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

        public static void ConfigureLoggerManager(this IServiceCollection services)
        {  
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void AddMiningVehicle(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BatteryConfiguration>(configuration.GetSection("BatteryConfiguration"));
            services.Configure<MotorConfiguration>(configuration.GetSection("MotorConfiguration"));

            services.AddSingleton<Battery>();
            services.AddSingleton<Motor>();
            services.AddSingleton<IMiningVehicleEmulator, MiningVehicleEmulator>();
        }

        private static string GetHubUrl()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return environment == "Development"
                ? "http://localhost:5140/vehicleDataHub"
                : "https://mining-vehicle.azurewebsites.net/vehicleDataHub";
        }
    }
}
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
                builder.WithOrigins("http://localhost:5173") // React app URL
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            }); 

        public static IServiceCollection AddSignalRClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<HubConnection>(provider =>
            {
                var hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://mining-vehicle.azurewebsites.net/vehicleDataHub")
                    .WithAutomaticReconnect()
                    .Build();

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
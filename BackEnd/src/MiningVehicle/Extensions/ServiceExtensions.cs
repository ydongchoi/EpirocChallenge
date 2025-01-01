using MiningVehicle.VehicleEmulator;
using MiningVehicle.VehicleEmulator.Components;
using MiningVehicle.VehicleEmulator.ConfigurationModels;

namespace MiningVehicle.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddMiningVehicle(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BatteryConfiguration>(configuration.GetSection("BatteryConfiguration"));
            services.Configure<MotorConfiguration>(configuration.GetSection("MotorConfiguration"));

            services.AddSingleton<Battery>();
            services.AddSingleton<Motor>();
            services.AddSingleton<MinigVehicleEmulator>();
        }
    }
}
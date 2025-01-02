using MiningVehicle.Infrastructure.Models;

namespace MiningVehicle.Infrastructure.Repositories
{
    public interface IRepository
    {
        Task AddVehicleDataAsync(VehicleData vehicleData);
    }
}
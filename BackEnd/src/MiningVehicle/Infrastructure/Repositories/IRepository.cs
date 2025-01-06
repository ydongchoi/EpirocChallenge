using MiningVehicle.Infrastructure.Models;

namespace MiningVehicle.Infrastructure.Repositories
{
    public interface IRepository
    {
        Task AddVehicleDataAsync(List<VehicleData> vehicleData);

        Task InsertOneVehicleDataAsync(VehicleData vehicleData);
    }
}
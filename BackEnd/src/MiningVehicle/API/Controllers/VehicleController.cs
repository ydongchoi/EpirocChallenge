using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using MiningVehicle.API.DTO;
using MiningVehicle.API.Services;
using MiningVehicle.VehicleEmulator;

namespace MiningVehicle.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("adjustSpeed")]
        public async Task<IActionResult> AdjustSpeed([FromBody] AdjustSpeedDTO adjustSpeedDTO)
        {
            await _vehicleService.AdjustSpeed(adjustSpeedDTO.Speed);

            return Ok($"Speed adjusted to {adjustSpeedDTO.Speed}");
        }

        [HttpPost("break")]
        public IActionResult Break()
        {
            return Ok();
        }

        [HttpPost("chargeBattery")]
        public IActionResult ChargeBattery()
        {

            return Ok();
        }
    }
}
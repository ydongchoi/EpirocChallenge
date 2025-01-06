using Microsoft.AspNetCore.Mvc;
using MiningVehicle.API.DTO;
using MiningVehicle.API.Services;

namespace MiningVehicle.API.Controllers
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

        [HttpPost("adjust-speed")]
        public async Task<IActionResult> AdjustSpeed([FromBody] AdjustSpeedDTO adjustSpeedDTO)
        {
            if (adjustSpeedDTO == null)
            {
                return BadRequest("Invalid speed adjustment data.");
            }

            try
            {
                await _vehicleService.AdjustSpeed(adjustSpeedDTO.Speed);
                return Ok($"Speed adjusted to {adjustSpeedDTO.Speed}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("break")]
        public IActionResult Break()
        {
            try
            {
                _vehicleService.Break();
                return Ok("Vehicle break applied.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("charge-battery")]
        public IActionResult ChargeBattery([FromBody] ChargeBatteryDTO chargeBatteryDTO)
        {
            if (chargeBatteryDTO == null)
            {
                return BadRequest("Invalid battery charging data.");
            }

            try
            {
                if (chargeBatteryDTO.IsCharging)
                {
                    _vehicleService.ChargeBattery();
                }
                else
                {
                    _vehicleService.StopBatteryCharging();
                }

                return Ok("Battery charging status changed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
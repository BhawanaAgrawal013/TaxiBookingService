namespace TaxiBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VehicleTypeController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleType;
        private readonly ILog logger;
        public VehicleTypeController(IVehicleTypeService vehicleType, ILog logger)
        {
            _vehicleType = vehicleType;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var vehicleTypes = _vehicleType.Display();
                if (vehicleTypes.Count == 0)
                {
                    logger.Warning("There are no Vehicle Tyoes present");
                    return NotFound();
                }
                logger.Information("Vehicle Types fetched from database successfully");
                return Ok(vehicleTypes);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<VehicleTypeDTO> Search(int id)
        {
            try
            {
                var type = _vehicleType.Search(id);
                if (type == null)
                {
                    logger.Error($"{id} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{id} found sucessfully");
                    return Ok(type);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(VehicleTypeDTO vehicleType)
        {
            try
            {
                if (_vehicleType.Add(vehicleType))
                {
                    logger.Information("Vehicle type added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Vehicle type could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Edit(VehicleTypeDTO type, int id)
        {
            try
            {
                if (_vehicleType.Update(type, id))
                {
                    logger.Information($"{id} Vehicle type updated sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error($"{id} could not be updated");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_vehicleType.Delete(id))
                { 
                    logger.Information($"Vehicle type {id} removed");
                    return Ok();
                }
                else
                {
                    logger.Error($"{id} not found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

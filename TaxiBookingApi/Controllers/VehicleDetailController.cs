namespace TaxiBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VehicleDetailController : ControllerBase
    {
        private readonly IVehicleDetailsService _vehicleDetail;
        private readonly ILog logger;
        public VehicleDetailController(IVehicleDetailsService vehicleDetail, ILog logger)
        { 
            _vehicleDetail = vehicleDetail;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var vehicleDetails = _vehicleDetail.Display();
                if (vehicleDetails.Count == 0)
                {
                    logger.Warning("There are no Vehicle Details present");
                    return NotFound();
                }
                logger.Information("Vehicle Details fetched from database successfully");
                return Ok(vehicleDetails);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public ActionResult<VehicleDetailResponse> Search([FromRoute] string email)
        {
            try
            {
                var detail = _vehicleDetail.Search(email);
                if (detail == null)
                {
                    logger.Error($"{email} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{email} found sucessfully");
                    return Ok(detail);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(VehicleDetailDTO vehicleDetail)
        {
            try
            {
                if (_vehicleDetail.Add(vehicleDetail))
                {
                    logger.Information("Vehicle Detail added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Vehicle Detail could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{email}")]
        [Authorize(Roles = "Driver")]
        public IActionResult Edit(VehicleDetailDTO detail, string email)
        {
            try
            {
                if (_vehicleDetail.Update(detail, email))
                {
                    logger.Information($"{email} Vehicle Detail updated sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error($"{email} could not be updated");
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
                if (_vehicleDetail.Delete(id))
                {
                    logger.Information($"Vehicle Detail {id} removed");
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

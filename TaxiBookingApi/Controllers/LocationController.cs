namespace TaxiBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _location;
        private readonly ILog logger;
        public LocationController(ILocationService location, ILog logger)
        {
            _location = location;
            this.logger = logger;
        }

        [HttpGet("{email}")]
        public ActionResult<LocationDTO> Search(string email)
        {
            try
            {
                var location = _location.Search(email);
                if (location == null)
                {
                    logger.Error($"{email} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{email} found sucessfully");
                    return Ok(location);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Driver")]
        public IActionResult Add(LocationDTO location)
        {
            try
            {
                if (_location.Add(location))
                {
                    logger.Information("Location added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Location could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{email}")]
        public IActionResult Edit(LocationDTO locationDTO, string email)
        {
            try
            {
                if (_location.Update(locationDTO, email))
                {
                    logger.Information($"{email} Location updated sucessfully");
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
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_location.Delete(id))
                {
                    logger.Information($"Location {id} removed");
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
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

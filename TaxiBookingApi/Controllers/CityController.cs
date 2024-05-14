using Microsoft.AspNetCore.Authorization;

namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _city;
        private readonly ILog logger;
        public CityController(ICityService city, ILog logger)
        {
            _city = city;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var city = _city.Display();
                if (city.Count == 0)
                {
                    logger.Warning("There are no Cities present");
                    return NotFound();
                }
                logger.Information("Cities fetched from database successfully");
                return Ok(city);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> Search(int id)
        {
            try
            {
                var city = _city.Search(id);
                if (city == null)
                {
                    logger.Error($"{id} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{id} found sucessfully");
                    return Ok(city);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception occured" + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(CityDTO city)
        {
            try
            {
                if (_city.Add(city))
                {
                    logger.Information("City added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("City could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Edit(CityDTO city, int id)
        {
            try
            {
                if (_city.Update(city, id))
                {
                    logger.Information($"{id} city updated sucessfully");
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
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_city.Delete(id))
                {
                    logger.Information($"City {id} removed");
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

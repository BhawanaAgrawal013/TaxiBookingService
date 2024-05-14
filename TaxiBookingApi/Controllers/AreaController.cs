namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _area;
        private readonly ILog logger;
        public AreaController(IAreaService area, ILog logger)
        {
            _area = area;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var areas = _area.Display();
                if (areas.Count == 0)
                {
                    logger.Warning("There are no Areas present");
                    return NotFound("There are no Areas present");
                }
                logger.Information("Areas fetched from database successfully");
                return Ok(areas);
            }
            catch (Exception ex) 
            {
                logger.Error(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AreaResponse> Search(int id)
        {
            try
            {
                var area = _area.Search(id);
                if (area == null)
                {
                    logger.Error($"{id} could not be found");
                    return NotFound($"{id} could not be found");
                }
                else
                {
                    logger.Information($"{id} found sucessfully");
                    return Ok(area);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception occured");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(AreaDTO area)
        {
            try
            {
                if (_area.Add(area))
                {
                    logger.Information("Area added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Area could not be added");
                    return BadRequest("Area could not be added, recheck your entries");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(AreaDTO area, int id)
        {
            try
            {
                if (_area.Update(area, id))
                {
                    logger.Information($"{id} area updated sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error($"{id} could not be updated");
                    return NotFound($"{id} could not be updated, recheck your entries");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_area.Delete(id))
                {
                    logger.Information($"Area {id} removed");
                    return Ok();
                }
                else
                {
                    logger.Error($"{id} not found");
                    return NotFound($"{id} not found");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

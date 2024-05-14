namespace TaxiBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StateController : ControllerBase
    {
        private readonly IStateService _state;
        private readonly ILog logger;
        public StateController(IStateService state, ILog logger)
        {
            _state = state;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var message = Ok(_state.Display());
                logger.Information("States fetched from database successfully");
                return message;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<StateDTO> Search(int id)
        {
            try
            {
                var state = _state.Search(id);
                if (state == null)
                {
                    logger.Error($"{id} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{id} found sucessfully");
                    return Ok(state);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult Add(StateDTO state)
        {
            try
            {
                if (_state.Add(state))
                {
                    logger.Information("State added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("State could not be added");
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
        public IActionResult Edit(StateDTO state, int id)
        {
            try
            {
                if (_state.Update(state, id))
                {
                    logger.Information($"{id} state updated sucessfully");
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_state.Delete(id))
                {
                    logger.Information($"State {id} removed");
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

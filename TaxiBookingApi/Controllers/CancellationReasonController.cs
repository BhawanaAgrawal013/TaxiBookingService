using Microsoft.AspNetCore.Authorization;

namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CancellationReasonController : ControllerBase
    {
        private readonly ICancellationReasonService _cancellation;
        private readonly ILog logger;
        public CancellationReasonController(ICancellationReasonService cancellation, ILog logger)
        {
            _cancellation = cancellation;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var cancellationReasons = _cancellation.Display();
                if (cancellationReasons.Count == 0)
                {
                    logger.Warning("There are no Cancellation Reason present");
                    return NotFound();
                }
                logger.Information("Cancellation Reason fetched from database successfully");
                return Ok(cancellationReasons);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(CancellationReasonDTO cancellationReason)
        {
            try
            {
                if (_cancellation.Add(cancellationReason))
                {
                    logger.Information("Cancellation Reason added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Cancellation Reason could not be added");
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
        public IActionResult Edit(CancellationReasonDTO reason, int id)
        {
            try
            {
                if (_cancellation.Update(reason, id))
                {
                    logger.Information($"{id} Cancellation Reason updated sucessfully");
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
                if (_cancellation.Delete(id))
                {
                    logger.Information($"Cancellation Reason {id} removed");
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

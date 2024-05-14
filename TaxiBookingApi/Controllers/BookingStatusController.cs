namespace TaxiBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class BookingStatusController : ControllerBase
    {
        private readonly IBookingStatusService _bookingstatus;
        private readonly ILog logger;
        public BookingStatusController(IBookingStatusService bookingStatus, ILog logger)
        {
            _bookingstatus = bookingStatus;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var bookingStatuses = _bookingstatus.Display();
                if (bookingStatuses.Count == 0)
                {
                    logger.Warning("There are no Booking Status present");
                    return NotFound();
                }
                logger.Information("Booking Status fetched from database successfully");
                return Ok(bookingStatuses);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<BookingStatusDTO> Search(int id)
        {
            try
            {
                var status = _bookingstatus.Search(id);
                if (status == null)
                {
                    logger.Error($"{id} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{id} found sucessfully");
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(BookingStatusDTO bookingStatus)
        {
            try
            {
                if (_bookingstatus.Add(bookingStatus))
                {
                    logger.Information("Booking Status added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Booking Status could not be added");
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
        public IActionResult Edit(BookingStatusDTO statusdto, int id)
        {
            try
            {
                if (_bookingstatus.Update(statusdto, id))
                {
                    logger.Information($"{id} Booking Status updated sucessfully");
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
        public IActionResult Delete(int id)
        {
            try
            {
                if (_bookingstatus.Delete(id))
                {
                    logger.Information($"Booking Status {id} removed");
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

namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingRepo;
        private readonly ICancelledBookingService _cancelledBooking;
        private readonly ILog logger;
        public BookingController(IBookingService bookingRepo, ILog logger, ICancelledBookingService cancelledBooking)
        {
            _bookingRepo = bookingRepo;
            this.logger = logger;
            _cancelledBooking = cancelledBooking;
        }


        [HttpGet("{id}")]
        public ActionResult<BookingDTO> Search(int id)
        {
            try
            {
                var booking = _bookingRepo.Search(id);
                if (booking == null)
                {
                    logger.Error($"{id} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{id} found sucessfully");
                    return Ok(booking);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public ActionResult<BookingDTO> GetCancelledRides(string email)
        {
            try
            {
                var booking = _cancelledBooking.Search(email);
                if (booking == null)
                {
                    logger.Error($"Cancelled bookings could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"Cancelled bookings found sucessfully");
                    return Ok(booking);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public ActionResult<BookingDTO> BookingHistory(string email)
        {
            try
            {
                var booking = _bookingRepo.History(email);
                if (booking == null)
                {
                    logger.Error($"bookings could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"bookings found sucessfully");
                    return Ok(booking);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<BookingDTO> Disputes()
        {
            try
            {
                var booking = _bookingRepo.Complaints();
                if (booking == null)
                {
                    logger.Error($"bookings could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"bookings found sucessfully");
                    return Ok(booking);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(BookingDTO booking)
        {
            try
            {
                var id = _bookingRepo.Add(booking);
                if (id != 0)
                {
                    logger.Information("Booking added sucessfully");
                    return Ok(id);
                }
                else
                {
                    logger.Error("Booking could not be added");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/{email}")]
        public IActionResult PostRequest(int id, string email)
        {
            try
            {
                if(_bookingRepo.PostRequest(id, email))
                {
                    logger.Information("Booking request posted successfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Booking could not be posted");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/{email}")]
        [Authorize(Roles = "Driver")]
        public IActionResult AcceptRequest(int id, string email)
        {
            try
            {
                var booking = _bookingRepo.AcceptRequest(id, email);
                if (booking != null)
                {
                    logger.Information("Booking added sucessfully");
                    return Ok(booking);
                }
                else
                {
                    logger.Error("Booking could not be added");
                    return Problem();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public IActionResult CancelRequest(int id)
        {
            try
            {
                if (_bookingRepo.CancelRequest(id))
                {
                    logger.Information("Booking cancelled sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Booking could not be cancelled");
                    return Problem();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Driver")]
        public IActionResult ArrivedLocation(int id)
        {
            try
            {
                if (_bookingRepo.ArrivedAtLocation(id))
                {
                    logger.Information("Booking arrived at location sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Booking did not arrive at location");
                    return Problem();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}/{reason}")]
        [Authorize(Roles = "User")]
        public IActionResult DeclineRequest(int id, string reason)
        {
            try
            {
                var booking = _bookingRepo.DeclineRequest(id, reason);
                if (booking != null)
                {
                    logger.Information("Booking added sucessfully");
                    return Ok(booking);
                }
                else
                {
                    logger.Error("Booking could not be added");
                    return Problem();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Driver")]
        public IActionResult CompleteRide(int id)
        {
            try
            {
                var booking = _bookingRepo.CompleteRide(id);
                if (booking != null)
                {
                    logger.Information("Booking added sucessfully");
                    return Ok(booking);
                }
                else
                {
                    logger.Error("Booking could not be added");
                    return Problem();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{email}")]
        public IActionResult PayCancellationFee(string email)
        {
            try
            {   
                if (_cancelledBooking.PayCancellationFee(email))
                {
                    logger.Information("Paid cancelling fee done sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Paid cancelling fee could not be done");
                    return Problem();
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

namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driver;
        private readonly ILog logger;
        private readonly IJwtService _jwtService;
        public DriverController(IDriverService driver, ILog logger, IJwtService jwtService)
        {
            _driver = driver;
            this.logger = logger;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var users = _driver.Display();
                if (users.Count == 0)
                {
                    logger.Warning("There are no Driver present");
                    return NotFound();
                }
                logger.Information("Driver fetched from database successfully");
                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public ActionResult<DriverDTO> Search(string email)
        {
            try
            {
                var driver = _driver.Search(email);
                if (driver == null)
                {
                    logger.Error($"{email} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{email} found sucessfully");
                    return Ok(driver);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public ActionResult<DriverDTO> CurrentDriver(string email)
        {
            try
            {
                var driver = _driver.CurrentDriver(email);
                if (driver == null)
                {
                    logger.Error($"{email} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{email} found sucessfully");
                    return Ok(driver);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public IActionResult GettingRequest(string email)
        {
            try
            {
                var bookings = _driver.GetingBookingRequest(email);

                if (bookings != null)
                {
                    logger.Information("Driver logged in successfully");
                    return Ok(bookings);
                }
                else
                {
                    logger.Error($"Driver could not be found");
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "Admin")]
        public ActionResult ApproveDriver(string email)
        {
            try
            {
                if (_driver.ApproveDriver(email))
                {
                    logger.Information($"{email} approved sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error($"{email} could not be found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{latitude}/{longitude}")]
        public IActionResult AvailableDriver([FromRoute] decimal latitude, [FromRoute] decimal longitude)
        {
            try
            {
                var vehicles = _driver.GetAvailableVehicles(latitude, longitude);

                if (vehicles != null)
                {
                    logger.Information("Driver logged in successfully");
                    return Ok(vehicles);
                }
                else
                {
                    logger.Error($"Driver could not be found");
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Add(DriverDTO driver)
        {
            try
            {
                if (_driver.Add(driver))
                {
                    logger.Information("Driver added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Driver could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Edit(DriverEditModel driver, string email)
        {
            try
            {
                if (_driver.Update(driver, email))
                {
                    logger.Information($"{email} driver updated sucessfully");
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
                if (_driver.Delete(id))
                {
                    logger.Information($"Driver {id} removed");
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

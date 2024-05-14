namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentModeController : ControllerBase
    {
        private readonly IPaymentModeService _paymentMode;
        private readonly ILog logger;
        public PaymentModeController(IPaymentModeService paymentMode, ILog logger)
        {
            _paymentMode = paymentMode;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var modes = _paymentMode.Display();
                if (modes.Count == 0)
                {
                    logger.Warning("There are no Payment Modes present");
                    return NotFound();
                }
                logger.Information("Payment Modes fetched from database successfully");
                return Ok(modes);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PaymentModeDTO> Search(int id)
        {
            try
            {
                var mode = _paymentMode.Search(id);
                if (mode == null)
                {
                    logger.Error($"{id} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{id} found sucessfully");
                    return Ok(mode);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(PaymentModeDTO paymentMode)
        {
            try
            {
                if (_paymentMode.Add(paymentMode))
                {
                    logger.Information("Payment Mode added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Payment Mode could not be added");
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
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(PaymentModeDTO mode, int id)
        {
            try
            {
                if (_paymentMode.Update(mode, id))
                {
                    logger.Information($"{id} Payment Mode updated sucessfully");
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
                if (_paymentMode.Delete(id))
                {
                    logger.Information($"Payment Mode {id} removed");
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

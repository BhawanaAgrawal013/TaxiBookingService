using DataAccessLayer.ResponseModels;

namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILog logger;
        public PaymentController(IPaymentService paymentService, ILog logger)
        {
            _paymentService = paymentService;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Add(PaymentDTO paymentDTO)
        {
            try
            {
                var payment = _paymentService.Add(paymentDTO);
                if (payment != 0)
                {
                    logger.Information("Payment added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Payment could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddWallet(int amount, string email)
        {
            try
            {
                if (_paymentService.AddWallet(amount, email))
                {
                    logger.Information("Payment added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Payment could not be added");
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
        public IActionResult Edit(PaymentDTO payment, int id)
        {
            try
            {
                if (_paymentService.Update(payment, id))
                {
                    logger.Information($"{id} Payment updated sucessfully");
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

        [HttpPut("{id}")]
        public IActionResult PaymentVerify(int id, PaymentVerificationModel verificationModel)
        {
            try
            {
                if (_paymentService.PaymentVerify(id, verificationModel))
                {
                    logger.Information($"{id} Payment verified sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error($"{id} could not be verified");
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

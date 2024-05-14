namespace TaxiBookingApi
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILog logger;
        private readonly IJwtService _jwtService;
        public const string SessionToken = "_token";
        public LoginController(ILog logger, IJwtService jwtService)
        {
            this.logger = logger;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult RefreshToken()
        {
            try
            {
                var token = _jwtService.RefreshToken();
                logger.Information("Driver logged in successfully");
                return Ok(token);
            }
            catch (Exception ex)
            {
                logger.Error("Exception: " + ex.Message + " Stack Trace: " + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{Email}/{password}")]
        public IActionResult Login([FromRoute] string Email, [FromRoute] string password)
        {
            try
            {
                if (_jwtService.Login(Email, password))
                {
                    var token = _jwtService.GenerateToken(Email, password);
                    logger.Information("Driver logged in successfully");
                    return Ok(token);
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
    }
}

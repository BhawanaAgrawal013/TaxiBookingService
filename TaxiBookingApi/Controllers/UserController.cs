namespace TaxiBookingApi
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly ILog logger;
        private readonly IJwtService _jwtService;
        public UserController(IUserService user, ILog logger, IJwtService jwtService)
        {
            _user = user;
            this.logger = logger;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var users = _user.Display();
                if (users.Count == 0)
                {
                    logger.Warning("There are no Users present");
                    return NotFound();
                }
                logger.Information("Users fetched from database successfully");
                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public ActionResult<UserEditModel> Search(string email)
        {
            try
            {
                var user = _user.Get(email);
                if (user == null)
                {
                    logger.Error($"{email} could not be found");
                    return NotFound();
                }
                else
                {
                    logger.Information($"{email} found sucessfully");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(UserDTO user)
        {
            try
            {
                if (_user.Add(user))
                {
                    logger.Information("User added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("User could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{email}")]
        [Authorize(Roles = "User")]
        public IActionResult Edit(UserEditModel user, string email)
        {
            try
            {
                if (_user.Update(user, email))
                {
                    logger.Information($"{email} user updated sucessfully");
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
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(string email)
        {
            try
            {
                if (_user.Delete(email))
                {
                    logger.Information($"User {email} removed");
                    return Ok();
                }
                else
                {
                    logger.Error($"{email} not found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.Error("exception occured" + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

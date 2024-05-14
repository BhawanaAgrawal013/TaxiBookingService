namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _role;
        private readonly ILog logger;
        public RoleController(IRoleService role, ILog logger)
        {
            _role = role;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Display()
        {
            try
            {
                var roles = _role.Display();
                if(roles.Count == 0)
                {
                    logger.Warning("There are no Roles present");
                    return NotFound();
                }
                logger.Information("Roles fetched from database successfully");
                return Ok(roles);
            }
            catch (Exception ex) 
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(RoleDTO role)
        {
            try
            {
                if(_role.Add(role))
                {
                    logger.Information("Role added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Role could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("id")]
        public IActionResult Edit(RoleDTO role, int id)
        {
            try
            {
                if (_role.Update(role, id))
                {
                    logger.Information("Role added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("Role could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                if(_role.Delete(id))
                {
                    logger.Information($"Role {id} removed");
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

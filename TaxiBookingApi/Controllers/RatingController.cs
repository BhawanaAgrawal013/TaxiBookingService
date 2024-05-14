namespace TaxiBookingApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _rating;
        private readonly ILog logger;
        public RatingController(IRatingService rating, ILog logger)
        {
            _rating = rating;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Add(RatingDTO rating)
        {
            try
            {
                if (_rating.Add(rating))
                {
                    logger.Information("rating added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("rating could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception occured");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UserRating(RatingDTO rating)
        {
            try
            {
                if (_rating.UserRating(rating))
                {
                    logger.Information("rating added sucessfully");
                    return Ok();
                }
                else
                {
                    logger.Error("rating could not be added");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception occured");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Edit(RatingDTO ratingDTO, int id)
        {
            try
            {
                if (_rating.Update(ratingDTO, id))
                {
                    logger.Information($"{id} rating updated sucessfully");
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
    }
}

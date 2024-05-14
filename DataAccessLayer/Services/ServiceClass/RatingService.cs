namespace DataAccessLayer
{
    public class RatingService : IRatingService
    { 
        private readonly IRatingUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public RatingService(IRatingUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }


        public decimal CalculateRating(int id)
        {
            try
            {
                List<Rating> ratings = _unitOfWork.Ratings.GetAll().FindAll(item => item.DriverId == id);

                decimal totalRating = 0;

                foreach (var rating in ratings)
                {
                    totalRating += rating.Ratings;
                }
                return totalRating/ratings.Count;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }
        }

        public bool UserRating(RatingDTO ratingdto)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == ratingdto.Email)) return false;

                User user = _unitOfWork.Users.Find(item => item.Email == ratingdto.Email);

                Booking booking = _unitOfWork.Bookings.GetAll().Find(item => item.Id == ratingdto.BookingId);

                Rating rating = new()
                {
                    Ratings = ratingdto.Ratings,
                    Comment = ratingdto.Comment,
                    UserId = (int)_unitOfWork.Users.Get(e => e.Email == ratingdto.Email).Id,
                    CreatedBy = (int)_unitOfWork.Users.Get(e => e.Email == ratingdto.Email).Id,
                    DriverId = booking.DriverId,
                    CreatedAt = DateTime.Now
                };

                _unitOfWork.Ratings.Add(rating);
                _unitOfWork.Complete();

                booking.UserRatingId = rating.Id;

                _unitOfWork.Bookings.Update(booking);
                _unitOfWork.Complete();

                user.AverageRating = (user.AverageRating + ratingdto.Ratings) / 2;
                _unitOfWork.Complete();

                _log.Information("RatingRepo: Rating added succesfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Add(RatingDTO ratingdto)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == ratingdto.Email)) return false;

                User user = _unitOfWork.Users.Find(item => item.Email == ratingdto.Email);

                Rating rating = new()
                {
                    Ratings = ratingdto.Ratings,
                    Comment = ratingdto.Comment,
                    DriverId = (int)_unitOfWork.Drivers.GetAll().Find(item => item.User.Email == ratingdto.Email).Id,
                    CreatedBy = (int)_unitOfWork.Users.Get(item => item.Email == ratingdto.Email).Id,
                    CreatedAt = DateTime.Now
                };

                _unitOfWork.Ratings.Add(rating);
                _unitOfWork.Complete();

                Booking booking = _unitOfWork.Bookings.Find(item => item.Id ==  ratingdto.BookingId);
                booking.RatingId = rating.Id;

                _unitOfWork.Bookings.Update(booking);
                _unitOfWork.Complete();

                user.AverageRating = (user.AverageRating + ratingdto.Ratings) / 2;
                _unitOfWork.Complete();

                _log.Information("RatingRepo: Rating added succesfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Update(RatingDTO ratingdto, int id)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == ratingdto.Email)) return false;

                User user = _unitOfWork.Users.Get(item => item.Email == ratingdto.Email);
                    
                Rating rating = _unitOfWork.Ratings.GetAll().Find(item => item.Id == id);

                rating.Ratings = ratingdto.Ratings;
                rating.Comment = ratingdto.Comment;
                rating.DriverId = _unitOfWork.Drivers.GetAll().Find(item => item.User.Email == ratingdto.Email).Id;

                _unitOfWork.Ratings.Update(rating);
                _unitOfWork.Complete();

                _log.Information("Rating for booking updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }
    }
}

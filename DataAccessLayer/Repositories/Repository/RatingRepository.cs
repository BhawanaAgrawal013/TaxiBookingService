namespace DataAccessLayer.Repositories
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<Rating> GetAll()
        {
            var results = _dbContext.Ratings.Include(e => e.Booking).Include(e => e.Driver).ThenInclude(e => e.User).ToList();
            return results;
        }

    }
}

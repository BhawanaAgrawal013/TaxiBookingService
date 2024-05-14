namespace DataAccessLayer
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<Location> GetAll() 
        {
            var results = _dbContext.Locations.Include(e => e.Area).Include(e => e.Driver).ThenInclude(e => e.User).ToList();
            return results;
        }
    }
}

namespace DataAccessLayer
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<Driver> GetAll()
        {
            var results = _dbContext.Drivers.Include(e => e.VehicleDetail).ThenInclude(e => e.VehicleType).Include(e => e.User).ThenInclude(e => e.Role).Include(e => e.Location).ToList();
            return results;
        }
    }
}

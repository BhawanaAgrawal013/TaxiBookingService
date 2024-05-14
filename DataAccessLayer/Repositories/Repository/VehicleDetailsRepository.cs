
using DataAccessLayer.Repositories.IRepository;

namespace DataAccessLayer
{
    public class VehicleDetailsRepository : GenericRepository<VehicleDetail>, IVehicleDetailsRepository
    {
        public VehicleDetailsRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<VehicleDetail> GetAll()
        {
            var results = _dbContext.VehicleDetails.Include(e => e.VehicleType).Include(e => e.Driver).ThenInclude(e => e.User).ToList();
            return results;
        }
    }
}

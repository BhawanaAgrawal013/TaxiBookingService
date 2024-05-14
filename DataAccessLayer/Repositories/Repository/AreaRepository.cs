
namespace DataAccessLayer
{
    public class AreaRepository : GenericRepository<Area> , IAreaRepository
    {
        public AreaRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<Area> GetAll()
        {
            var results = _dbContext.Areas.Where(e => !e.isDelete).Include(e => e.City).ThenInclude(e => e.State).ToList();
            return results;
        }
    }
}

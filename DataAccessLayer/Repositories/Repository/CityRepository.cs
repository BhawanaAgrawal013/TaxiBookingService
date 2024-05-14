using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class CityRepository : GenericRepository<City> , ICityRepository
    {
        public CityRepository(TaxiBookingContext _context) : base(_context)
        {
        }
        public List<City> GetCities()
        {
            var result = _dbContext.Cities.Where(x => !x.isDelete).Include(x=>x.State).ToList();
            return result;
        }
    }
}

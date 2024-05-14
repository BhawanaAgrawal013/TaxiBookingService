
namespace DataAccessLayer
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(TaxiBookingContext _context) : base(_context)
        {
        }
    }
}


namespace DataAccessLayer
{
    public class StateRepository : GenericRepository<State> , IStateRepository
    {
        public StateRepository(TaxiBookingContext _context) : base(_context)
        {
        }
    }
}

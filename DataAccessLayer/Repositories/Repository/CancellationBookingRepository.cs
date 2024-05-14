namespace DataAccessLayer
{
    public class CancellationBookingRepository : GenericRepository<CancelledBooking> , ICancellationBookingRepository
    {
        public CancellationBookingRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<CancelledBooking> GetAll()
        {
            var results = _dbContext.CancelledBookings.Include(c => c.Booking).ThenInclude(e => e.Payment).Include(e => e.CancellationReason).Include(e => e.Booking).ThenInclude(e => e.User).ToList();
            return results;
        }
    }
}

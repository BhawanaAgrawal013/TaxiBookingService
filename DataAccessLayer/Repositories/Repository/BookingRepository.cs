namespace DataAccessLayer
{
    public class BookingRepository : GenericRepository<Booking> , IBookingRepository
    {
        public BookingRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<Booking> GetAll()
        {
            var result = _dbContext.Bookings.Include(e => e.User).Include(e => e.Rating).Include(e => e.Driver).ThenInclude(e => e.User).Include(e => e.Payment).ThenInclude(e => e.PaymentMode).Include(e => e.BookingStatus).ToList();
            return result;
        }
    }
}

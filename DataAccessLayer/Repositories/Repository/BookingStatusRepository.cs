namespace DataAccessLayer
{
    public class BookingStatusRepository : GenericRepository<BookingStatus> , IBookingStatusRepository
    {
        public BookingStatusRepository(TaxiBookingContext _context) : base(_context)
        {
        }
    }
}

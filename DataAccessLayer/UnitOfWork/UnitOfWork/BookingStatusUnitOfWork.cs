namespace DataAccessLayer
{
    public class BookingStatusUnitOfWork : IBookingStatusUnitOfWork
    {

        private readonly TaxiBookingContext _dBContext;

        public BookingStatusUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;
            Users = new UserRepository(_dBContext);
            BookingsStatus = new BookingStatusRepository(_dBContext);
        }
        public IUserRepository Users { get; }
        public IBookingStatusRepository BookingsStatus { get; }

        public void Complete()
        {
            _dBContext.SaveChanges();
        }

        public void Dispose()
        {
            _dBContext.Dispose();
        }
    }
}

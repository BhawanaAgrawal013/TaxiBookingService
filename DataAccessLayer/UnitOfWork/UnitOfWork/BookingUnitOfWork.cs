using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class BookingUnitOfWork : IBookingUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public BookingUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;
            Users = new UserRepository(_dBContext);
            Bookings = new BookingRepository(_dBContext);
            BookingsStatus = new BookingStatusRepository(_dBContext);
            Drivers = new DriverRepository(_dBContext);
        }
        public IUserRepository Users { get; }
        // public IRoleRepository UserRoles { get; }
        public IBookingRepository Bookings { get; }
        public IBookingStatusRepository BookingsStatus { get; }
        // public ICancellationReasonRepository CancellationReasons { get; }
        public IDriverRepository Drivers { get; }

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

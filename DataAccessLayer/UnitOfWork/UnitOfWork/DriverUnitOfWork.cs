namespace DataAccessLayer
{
    public class DriverUnitOfWork : IDriverUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public DriverUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            UserRoles = new RoleRepository(_dBContext);
            Drivers = new DriverRepository(_dBContext);
            Locations = new LocationRepository(_dBContext);
            Bookings = new BookingRepository(_dBContext);
        }

        public IUserRepository Users { get; }
        public IDriverRepository Drivers { get; }
        public ILocationRepository Locations { get; }
        public IRoleRepository UserRoles { get; }
        public IBookingRepository Bookings { get; }

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

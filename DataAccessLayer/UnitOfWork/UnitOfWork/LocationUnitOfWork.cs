namespace DataAccessLayer
{
    public class LocationUnitOfWork : ILocationUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public LocationUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            Locations = new LocationRepository(_dBContext);
            Areas = new AreaRepository(_dBContext);
            Drivers = new DriverRepository(_dBContext);
        }
        public IUserRepository Users { get; }
        public IDriverRepository Drivers { get; }
        public ILocationRepository Locations { get; }
        public IAreaRepository Areas { get; }

        public void Complete()
        {
            _dBContext.SaveChanges();
        }

    }
}

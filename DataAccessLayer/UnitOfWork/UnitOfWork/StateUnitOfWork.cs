namespace DataAccessLayer
{
    public class StateUnitOfWork : IStateUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public StateUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            States = new StateRepository(_dBContext);
            Users = new UserRepository(_dBContext);
            /*
            UserRoles = new RoleRepository(_dBContext);
            Bookings = new BookingRepository(_dBContext);
            Areas = new AreaRepository(_dBContext);
            Cities = new CityRepository(_dBContext);
            States = new StateRepository(_dBContext);
            BookingsStatus = new BookingStatusRepository(_dBContext);
            CancellationReasons = new CancellationReasonRepository(_dBContext);
            Drivers = new DriverRepository(_dBContext);
            VehicleCategories = new VehicleCategoryRepository(_dBContext);
            VehiclesDetails = new VehicleDetailsRepository(_dBContext);
            Locations = new LocationRepository(_dBContext);
            PaymentModes = new PaymentModeRepository(_dBContext);
            Payments = new PaymentRepository(_dBContext);
            CancellationBookings = new CancellationBookingRepository(_dBContext);
            Ratings = new RatingRepository(_dBContext);*/
        }
        public IStateRepository States { get; }
        public IUserRepository Users { get; }

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

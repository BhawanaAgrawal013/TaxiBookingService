namespace DataAccessLayer
{
    public interface IDriverUnitOfWork : IDisposable
    {
        void Complete();

        public IUserRepository Users { get; }
        public IDriverRepository Drivers { get; }
        public ILocationRepository Locations { get; }
        public IRoleRepository UserRoles { get; }
        public IBookingRepository Bookings { get; }
    }
}

namespace DataAccessLayer
{
    public interface ILocationUnitOfWork
    {
        void Complete();

        public IUserRepository Users { get; }
        public IDriverRepository Drivers { get; }
        public ILocationRepository Locations { get; }
        public IAreaRepository Areas { get; }
    }
}

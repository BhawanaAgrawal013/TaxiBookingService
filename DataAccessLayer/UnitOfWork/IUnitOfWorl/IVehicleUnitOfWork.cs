using DataAccessLayer.Repositories.IRepository;

namespace DataAccessLayer
{
    public interface IVehicleUnitOfWork
    {
        void Complete();

        public IUserRepository Users { get; }
        public IDriverRepository Drivers { get; }
        public IVehicleCategoryRepository VehicleCategories { get; }
        public IVehicleDetailsRepository VehiclesDetails { get; }
    }
}

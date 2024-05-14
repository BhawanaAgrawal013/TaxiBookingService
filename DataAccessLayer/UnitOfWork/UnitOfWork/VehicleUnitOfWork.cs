using DataAccessLayer.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class VehicleUnitOfWork : IVehicleUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public VehicleUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            Drivers = new DriverRepository(_dBContext);
            VehicleCategories = new VehicleCategoryRepository(_dBContext);
            VehiclesDetails = new VehicleDetailsRepository(_dBContext);
        }
        public IUserRepository Users { get; }
        public IDriverRepository Drivers { get; }
        public IVehicleCategoryRepository VehicleCategories { get; }
        public IVehicleDetailsRepository VehiclesDetails { get; }

        public void Complete()
        {
            _dBContext.SaveChanges();
        }
    }
}

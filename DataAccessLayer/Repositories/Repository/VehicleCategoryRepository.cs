namespace DataAccessLayer
{
    public class VehicleCategoryRepository : GenericRepository<VehicleType> , IVehicleCategoryRepository
    {
        public VehicleCategoryRepository(TaxiBookingContext _context) : base(_context)
        {
        }
    }
}

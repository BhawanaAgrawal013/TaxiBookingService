namespace DataAccessLayer.Repositories.IRepository
{
    public interface IVehicleDetailsRepository : IGenericRepository<VehicleDetail>
    {
        List<VehicleDetail> GetAll();
    }
}

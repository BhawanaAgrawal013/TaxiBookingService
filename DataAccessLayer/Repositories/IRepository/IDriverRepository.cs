namespace DataAccessLayer
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        List<Driver> GetAll();
    }
}

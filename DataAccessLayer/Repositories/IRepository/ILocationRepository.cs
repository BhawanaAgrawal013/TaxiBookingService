namespace DataAccessLayer
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        List<Location> GetAll();
    }
}

namespace DataAccessLayer
{
    public interface ICityRepository : IGenericRepository<City>
    {
        List<City> GetCities();
    }
}

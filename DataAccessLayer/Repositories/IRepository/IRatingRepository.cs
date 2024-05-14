namespace DataAccessLayer.Repositories
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        List<Rating> GetAll();
    }
}


namespace DataAccessLayer
{
    public interface IAreaRepository : IGenericRepository<Area>
    {
        List<Area> GetAll();
    }
}

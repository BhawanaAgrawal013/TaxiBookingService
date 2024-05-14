
namespace DataAccessLayer
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        List<Booking> GetAll();
    }
}

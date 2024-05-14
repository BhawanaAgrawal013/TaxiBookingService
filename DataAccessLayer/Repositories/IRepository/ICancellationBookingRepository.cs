namespace DataAccessLayer
{
    public interface ICancellationBookingRepository : IGenericRepository<CancelledBooking>
    {
        List<CancelledBooking> GetAll();
    }
}

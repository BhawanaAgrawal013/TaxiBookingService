namespace DataAccessLayer
{
    public interface IBookingStatusService
    {
        List<BookingStatusResponse> Display();
        bool Add(BookingStatusDTO newStatus);
        BookingStatusResponse Search(int id);
        bool Update(BookingStatusDTO updatedStatus, int id);
        bool Delete(int id);
    }
}

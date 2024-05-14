namespace DataAccessLayer
{
    public interface IBookingService
    {
        List<BookingResponse> History(string email);
        List<BookingResponse> Complaints();
        int Add(BookingDTO newBooking);
        public bool PostRequest(int id, string email);
        public BookingDTO Search(int id);
        public BookingDTO AcceptRequest(int id, string email);
        public bool CancelRequest(int id);
        public BookingDTO DeclineRequest(int id, string reason);
        public bool ArrivedAtLocation(int id);
        public BookingDTO CompleteRide(int id);
    }
}

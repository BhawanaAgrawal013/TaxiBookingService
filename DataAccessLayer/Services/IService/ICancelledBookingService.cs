namespace DataAccessLayer
{
    public interface ICancelledBookingService
    {
        public bool Add(int id, string reason, string userEmail);
        public List<CancelledBookingDTO> Search(string userEmail);
        public bool PaidCancellationFee(int id);
        public bool PayCancellationFee(string email);
    }
}

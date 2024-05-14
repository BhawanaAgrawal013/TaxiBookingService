namespace DataAccessLayer
{
    public interface ICancellationBookingUOW
    {
        void Complete();
        public IUserRepository Users { get; }
        public ICancellationReasonRepository CancellationReasons { get; }
        public ICancellationBookingRepository CancellationBookings { get; }
    }
}

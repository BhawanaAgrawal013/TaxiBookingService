namespace DataAccessLayer
{
    public interface IBookingUnitOfWork
    {
        void Complete();
        public IUserRepository Users { get; }
       // public IRoleRepository UserRoles { get; }
        public IBookingRepository Bookings { get; }
        public IBookingStatusRepository BookingsStatus { get; }
       // public ICancellationReasonRepository CancellationReasons { get; }
        public IDriverRepository Drivers { get; }
       // public IPaymentRepository Payments { get; }
        //public ICancellationBookingRepository CancellationBookings { get; }
    }
}

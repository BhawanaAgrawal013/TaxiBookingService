namespace DataAccessLayer
{
    public interface IPaymentUnitOfWork
    {
        void Complete();

        public IUserRepository Users { get; }
        public IPaymentModeRepository PaymentModes { get; }
        public IPaymentRepository Payments { get; }
        public IBookingRepository Bookings { get; }
    }
}

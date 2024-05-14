namespace DataAccessLayer
{
    public interface IBookingStatusUnitOfWork
    {
        void Complete();

        public IUserRepository Users { get; }
        public IBookingStatusRepository BookingsStatus { get; }
    }
}

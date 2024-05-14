using DataAccessLayer.Repositories;

namespace DataAccessLayer
{
    public interface IRatingUnitOfWork
    {
        void Complete();

        public IUserRepository Users { get; }
        public IBookingRepository Bookings { get; }
        public IDriverRepository Drivers { get; }
        public IRatingRepository Ratings { get; }

    }
}

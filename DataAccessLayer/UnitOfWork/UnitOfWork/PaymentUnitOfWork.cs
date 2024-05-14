using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class PaymentUnitOfWork : IPaymentUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public PaymentUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            PaymentModes = new PaymentModeRepository(_dBContext);
            Payments = new PaymentRepository(_dBContext);
            Bookings = new BookingRepository(_dBContext);
        }
        public IUserRepository Users { get; }
        public IPaymentModeRepository PaymentModes { get; }
        public IPaymentRepository Payments { get; }
        public IBookingRepository Bookings { get; }

        public void Complete()
        {
            _dBContext.SaveChanges();
        }
    }
}

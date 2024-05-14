namespace DataAccessLayer
{
    public class PaymentModeRepository : GenericRepository<PaymentMode>, IPaymentModeRepository
    {
        public PaymentModeRepository(TaxiBookingContext _context) : base(_context)
        {
        }
    }
}

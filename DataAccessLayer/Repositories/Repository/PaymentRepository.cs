namespace DataAccessLayer
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public List<Payment> GetAll()
        {
            var results = _dbContext.Payments.Include(e => e.Booking).Include(e => e.PaymentMode).ToList();
            return results;
        }
    }
}

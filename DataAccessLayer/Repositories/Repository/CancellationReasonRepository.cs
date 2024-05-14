namespace DataAccessLayer
{
    public class CancellationReasonRepository : GenericRepository<CancellationReason> , ICancellationReasonRepository
    {
        public CancellationReasonRepository(TaxiBookingContext _context) : base(_context)
        {
        }
    }
}

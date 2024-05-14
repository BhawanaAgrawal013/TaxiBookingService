namespace DataAccessLayer
{
    public class CancellationBookingUOW : ICancellationBookingUOW
    {
        private readonly TaxiBookingContext _dBContext;

        public CancellationBookingUOW(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            CancellationReasons = new CancellationReasonRepository(_dBContext);
            Users = new UserRepository(_dBContext);
            CancellationBookings = new CancellationBookingRepository(_dBContext);
        }

        public IUserRepository Users { get; }
        public ICancellationReasonRepository CancellationReasons { get; }
        public ICancellationBookingRepository CancellationBookings { get; }
        public void Complete()
        {
            _dBContext.SaveChanges();
        }

    }
}

namespace DataAccessLayer
{
    public class AreaUnitOfWork : IAreaUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public AreaUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            Areas = new AreaRepository(_dBContext);
            Cities = new CityRepository(_dBContext);
        }
        public IUserRepository Users { get; }
        public IAreaRepository Areas { get; }
        public ICityRepository Cities { get; }

        public void Complete()
        {
            _dBContext.SaveChanges();
        }

        public void Dispose()
        {
            _dBContext.Dispose();
        }
    }
}

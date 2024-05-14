namespace DataAccessLayer
{
    public class CityUnitOfWork : ICityUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public CityUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            Cities = new CityRepository(_dBContext);
            States = new StateRepository(_dBContext);

        }
        public IUserRepository Users { get; }
        public ICityRepository Cities { get; }
        public IStateRepository States { get; }

        public void Complete()
        {
            _dBContext.SaveChanges();
        }
    }
}

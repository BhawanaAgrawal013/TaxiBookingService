namespace DataAccessLayer
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        private readonly TaxiBookingContext _dBContext;

        public UserUnitOfWork(TaxiBookingContext dbcontext)
        {
            _dBContext = dbcontext;

            Users = new UserRepository(_dBContext);
            UserRoles = new RoleRepository(_dBContext);

        }
        public IUserRepository Users { get; }
        public IRoleRepository UserRoles { get; }
        public void Complete()
        {
            _dBContext.SaveChanges();
        }
    }
}

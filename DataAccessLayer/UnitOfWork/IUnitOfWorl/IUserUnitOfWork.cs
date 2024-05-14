namespace DataAccessLayer
{
    public interface IUserUnitOfWork
    {
        void Complete();

        public IUserRepository Users { get; }
        public IRoleRepository UserRoles { get; }
    }
}

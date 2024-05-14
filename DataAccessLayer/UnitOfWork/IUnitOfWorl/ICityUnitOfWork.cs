namespace DataAccessLayer
{
    public interface ICityUnitOfWork
    {
        void Complete();

        public IUserRepository Users { get; }
        public ICityRepository Cities { get; }
        public IStateRepository States { get; }
    }
}

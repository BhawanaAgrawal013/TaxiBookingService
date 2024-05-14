namespace DataAccessLayer
{
    public interface IStateUnitOfWork : IDisposable
    {
        void Complete();
        public IStateRepository States { get; }
        public IUserRepository Users { get; }
    }
}

namespace DataAccessLayer
{
    public interface IAreaUnitOfWork : IDisposable
    {
        void Complete();
        public IUserRepository Users { get; }
        public IAreaRepository Areas { get; }
        public ICityRepository Cities { get; }
    }
}

namespace DataAccessLayer
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        string GetRoleName(int roleId);
    }

}

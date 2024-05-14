namespace DataAccessLayer
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(TaxiBookingContext _context) : base(_context)
        {
        }

        public string GetRoleName(int roleId)
        {
            return FindAll(item => item.Id == roleId).Select(item => item.RoleName).FirstOrDefault();
        }
    }
}

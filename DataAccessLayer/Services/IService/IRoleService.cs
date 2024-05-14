namespace DataAccessLayer
{
    public interface IRoleService
    {
        List<RoleResponse> Display();
        bool Add(RoleDTO newRole);
       // UserDTO Search(int emoail);
        bool Update(RoleDTO updatedRole, int id);
        bool Delete(int id);
    }
}

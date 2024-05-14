namespace DataAccessLayer
{
    public interface IUserService
    {
        List<UserResponse> Display();
        bool Add(UserDTO newUser);
        UserEditModel Get(string email);
        bool Update(UserEditModel updatedUser, string email);
        bool Delete(string email);
        bool Login(string email, string password);
        bool AdminLogin(string email, string password);
    }
}

namespace DataAccessLayer
{
    public interface IJwtService
    {
        public string GenerateToken(string Email, string password);
        public string RefreshToken();
        public bool Login(string Email, string Password);
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAccessLayer
{
    public class JwtService : IJwtService
    {
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILog _log;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHashingService _hashingService;

        public JwtService(IUserUnitOfWork unitOfWork, ILog log, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHashingService hashingService)
        {
            _unitOfWork = unitOfWork;
            _log = log;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _hashingService = hashingService;
        }

        public bool Login(string Email, string Password)
        {
            try
            {
                User user = _unitOfWork.Users.Get(item => item.Email == Email);

                if (user == null) return false;

                bool isValidPassword = _hashingService.VerifyHashing(Password, user.Password);

                if (!isValidPassword)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public string GenerateToken(string Email, string password)
        {
            try
            {
                var user = _unitOfWork.Users.Get(e => e.Email == Email);

                string role = _unitOfWork.UserRoles.GetRoleName(user.RoleId);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public string RefreshToken()
        {
            try
            {
                var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var login = _unitOfWork.Users.Find(e => e.Email == claim);

                var response = GenerateToken(login.Email, login.Password);

                if (response == null)
                    return null;

                return response;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }
    }
}

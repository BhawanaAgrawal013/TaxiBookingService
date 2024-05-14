namespace DataAccessLayer
{
    public class UserService : IUserService
    {
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly IHashingService _hashingService;
        private readonly ILog _log;

        public UserService(IUserUnitOfWork unitOfWork, IHashingService hashingService, ILog log)
        {
            _unitOfWork = unitOfWork;
            _hashingService = hashingService;
            _log = log;
        }

        public List<UserResponse> Display()
        {
            try
            {
                var users = _unitOfWork.Users.Display(item => item.isDelete == false);

                List<UserResponse> userDTOs = new List<UserResponse>();

                foreach (var user in users)
                {
                    Role userRole = _unitOfWork.UserRoles.Get(item => item.Id == user.RoleId);

                    UserResponse userDTO = new()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DateOfBirth = user.DateOfBirth,
                        Email = user.Email,
                        Gender = user.Gender,
                        PhoneNumber = user.PhoneNumber,
                        RoleName = userRole.RoleName,
                    };
                    userDTOs.Add(userDTO);
                }

                _log.Information("UserRepo: user list displayed successfully");
                return userDTOs;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(UserDTO newUser)
        {
            try
            {
                if (_unitOfWork.Users.Exists(item => item.Email == newUser.Email))
                {
                    User existingUser = _unitOfWork.Users.Get(item => item.Email == newUser.Email);
                    if (existingUser.isDelete == false || existingUser == null)
                    {
                        return false;
                    }
                }

                Role userRole = _unitOfWork.UserRoles.Get(item => item.RoleName == newUser.RoleName);

                if (userRole == null) return false;

                User user = new()
                {
                    Email = newUser.Email,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Gender = newUser.Gender,
                    PhoneNumber = newUser.PhoneNumber,
                    Password = _hashingService.Hashing(newUser.Password),
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    DateOfBirth = newUser.DateOfBirth,
                    RoleId = userRole.Id,
                    AverageRating = 0
                };

                _unitOfWork.Users.Add(user);
                _unitOfWork.Complete();

                _log.Information("UserRepo: user added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public UserEditModel Get(string email)
        {
            try
            {
                User user = _unitOfWork.Users.Get(item => item.Email == email);

                Role userRole = _unitOfWork.UserRoles.Get(item => item.Id == user.RoleId);

                if(userRole.Id != user.RoleId) return null;

                UserEditModel userDTO = new()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth, Email = user.Email, Gender = user.Gender, PhoneNumber = user.PhoneNumber,
                    Wallet = user.wallet,
                    Rating = user.AverageRating
                };

                _log.Information("UserRepo: User fetched successfully");
                return userDTO;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(UserEditModel updatedUser , string email)
        {
            try
            {
                User user = _unitOfWork.Users.Get(item => item.Email == email);

                if (user == null) return false;

                if (user.isDelete) return false;

                user.Email = updatedUser.Email;
                user.Gender = updatedUser.Gender;
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.DateOfBirth = updatedUser.DateOfBirth;
                user.ModifiedAt = DateTime.Now;
                user.ModifiedBy = user.Id;

                _unitOfWork.Users.Update(user);
                _unitOfWork.Complete();

                _log.Information("UserRepo: user updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Delete(string email)
        {
            try
            {
                User user = _unitOfWork.Users.Get(item => item.Email == email);

                if (user == null) return false;
                if (user.isDelete) return false;

                user.isDelete = true;

                _unitOfWork.Users.Update(user);
                _unitOfWork.Complete();

                _log.Information("UserRepo: User deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Login(string email, string password)
        {
            User user = _unitOfWork.Users.Get(item => item.Email == email);

            if (user == null) return false;

            string role = _unitOfWork.UserRoles.GetRoleName(user.RoleId);

            if (!(role == Roles.User.ToString())) return false;

            bool isValidPassword = _hashingService.VerifyHashing(password, user.Password);

            if (!isValidPassword)
                return false;

            return true;
        }

        public bool AdminLogin(string email, string password)
        {
            if (!_unitOfWork.Users.Exists(item => item.Email == email)) return false;
            
            User user = _unitOfWork.Users.Get(item => item.Email == email);

            if (user == null) return false;

            string role = _unitOfWork.UserRoles.GetRoleName(user.RoleId);

            if (!(role == Roles.Admin.ToString())) return false;

            bool isValidPassword = _hashingService.VerifyHashing(password, user.Password);

            if (!isValidPassword)
                return false;

            return true;
        }
    }
}

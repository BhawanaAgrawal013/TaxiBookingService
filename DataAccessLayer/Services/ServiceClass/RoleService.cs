namespace DataAccessLayer
{
    public class RoleService : IRoleService
    {
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public RoleService(IUserUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<RoleResponse> Display()
        {
            try
            {
                _log.Information("Role list fetched successfully");
                return _unitOfWork.UserRoles.Display(item => item.isDelete == false).Select(e => new RoleResponse { RoleName = e.RoleName, Id = e.Id }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(RoleDTO newRole)
        {
            try
            {
                bool userExists = _unitOfWork.Users.Exists(item => item.Email == newRole.UserEmail);

                if (!userExists) return false;

                if (_unitOfWork.UserRoles.Exists(item => item.RoleName == newRole.RoleName))
                {
                    Role existingRole = _unitOfWork.UserRoles.Get(item => item.RoleName == newRole.RoleName);

                    if (existingRole.isDelete == false)
                    {
                        return false;
                    }
                }
                User user = _unitOfWork.Users.Get(item => item.Email == newRole.UserEmail);

                Role role = new()
                {
                    RoleName = newRole.RoleName,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = user.Id
                };

                _unitOfWork.UserRoles.Add(role);
                _unitOfWork.Complete();

                _log.Information("RoleRepo: Role added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Update(RoleDTO updatedRole, int id)
        {
            try
            {
                Role role = _unitOfWork.UserRoles.Get(item => item.Id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == updatedRole.UserEmail);

                if (role == null) return false;

                if (role.isDelete) return false;

                role.RoleName = updatedRole.RoleName;
                role.ModifiedAt = DateTime.Now;
                role.ModifiedBy = user.Id;

                _unitOfWork.UserRoles.Update(role);
                _unitOfWork.Complete();

                _log.Information("RoleRepo: Role updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                Role role = _unitOfWork.UserRoles.Get(item => item.Id == id);

                if (role == null) return false;
                if (role.isDelete) return false;

                role.isDelete = true;

                _unitOfWork.UserRoles.Update(role);
                _unitOfWork.Complete();

                _log.Information("RoleRepo: Role deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }
    }
}

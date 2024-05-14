namespace DataAccessLayer { 
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IVehicleUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public VehicleTypeService(IVehicleUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<VehicleTypeResponse> Display()
        {
            try
            {
                _log.Information("Vehicle Tyep displayed successfully");
                return _unitOfWork.VehicleCategories.Display(item => item.isDelete == false).Select(e => new VehicleTypeResponse { VehicleType = e.Type, Id = e.Id }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(VehicleTypeDTO newCategory)
        {
            try
            {
                bool userExists = _unitOfWork.Users.Exists(item => item.Email == newCategory.UserEmail);

                if (!userExists) return false;

                if (_unitOfWork.VehicleCategories.Exists(item => item.Type == newCategory.VehicleType))
                {
                    VehicleType existingCategory = _unitOfWork.VehicleCategories.Get(item => item.Type == newCategory.VehicleType);

                    if (existingCategory.isDelete == false)
                    {
                        return false;
                    }
                }

                User user = _unitOfWork.Users.Get(item => item.Email == newCategory.UserEmail);

                VehicleType category = new()
                {
                    Type = newCategory.VehicleType,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.VehicleCategories.Add(category);
                _unitOfWork.Complete();

                _log.Information("Vehicle Type added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public VehicleTypeDTO Search(int id)
        {
            try
            {
                VehicleType type = _unitOfWork.VehicleCategories.Get(item => item.Id == id);

                if (type.isDelete || type == null) return null;

                VehicleTypeDTO vehicleType = new()
                {
                    VehicleType = type.Type
                };

                _log.Information("Vehicle Type fetched successfully");
                return vehicleType;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(VehicleTypeDTO updatedCategory, int id)
        {
            try
            {
                VehicleType category = _unitOfWork.VehicleCategories.Get(item => item.Id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == updatedCategory.UserEmail);

                if (category == null) return false;

                if (category.isDelete) return false;

                category.Type = updatedCategory.VehicleType;
                category.ModifiedAt = DateTime.Now;
                category.ModifiedBy = user.Id;

                _unitOfWork.VehicleCategories.Update(category);
                _unitOfWork.Complete();

                _log.Information("Vehicle Type updated succesfully");
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
                VehicleType category = _unitOfWork.VehicleCategories.Get(item => item.Id == id);

                if (category == null) return false;
                if (category.isDelete) return false;

                category.isDelete = true;

                _unitOfWork.VehicleCategories.Update(category);
                _unitOfWork.Complete();

                _log.Information("Vehicle Type deleted successfully");
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


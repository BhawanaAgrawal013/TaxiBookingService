namespace DataAccessLayer
{
    public class VehicleDetailsService : IVehicleDetailsService
    {
        private readonly IVehicleUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public VehicleDetailsService(IVehicleUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }
            
        public List<VehicleDetailResponse> Display()
        {
            try
            {
                var vehicles = _unitOfWork.VehiclesDetails.GetAll();

                List<VehicleDetailResponse> vehicleDetails = new List<VehicleDetailResponse>();

                foreach (var e in vehicles)
                {
                    VehicleDetailResponse vehicleDetailDTO = new()
                    {
                        Manufacturer = e.Manufacturer,
                        RegistrationName = e.RegistrationName,
                        RegistrationNumber = e.RegistrationNumber,
                        Seating = e.Seating,
                        VehicleModel = e.VehicleModel,
                        VehicleType = e.VehicleType.Type
                    };

                    vehicleDetails.Add(vehicleDetailDTO);
                }

                _log.Information("Vehicle Details displayed successfully");
                return vehicleDetails;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(VehicleDetailDTO newDetail)
        {
            try
            {
                bool userExists = _unitOfWork.Users.Exists(item => item.Email == newDetail.UserEmail);
                
                if (!userExists) return false;

                if (_unitOfWork.VehiclesDetails.Exists(item => item.RegistrationNumber == newDetail.RegistrationNumber))
                {
                    VehicleDetail existingDetail = _unitOfWork.VehiclesDetails.Get(item => item.RegistrationNumber == newDetail.RegistrationNumber);

                    if (existingDetail.isDelete == false)
                    {
                        return false;
                    }
                }

                User user = _unitOfWork.Users.Get(item => item.Email == newDetail.UserEmail);
                VehicleType vehicleCategory = _unitOfWork.VehicleCategories.Get(item => item.Type == newDetail.VehicleType);

                VehicleDetail detail = new()
                {
                    Manufacturer = newDetail.Manufacturer,
                    VehicleModel = newDetail.VehicleModel,
                    RegistrationNumber = newDetail.RegistrationNumber,
                    RegistrationName = newDetail.RegistrationName,
                    Seating = newDetail.Seating,
                    VehicleTypeId = vehicleCategory.Id,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.VehiclesDetails.Add(detail);
                _unitOfWork.Complete();

                Driver driver = _unitOfWork.Drivers.GetAll().Find(item => item.User.Email == newDetail.UserEmail);
                
                if (driver == null) return false;

                driver.VehicleId = detail.Id;
                _unitOfWork.Drivers.Update(driver);
                _unitOfWork.Complete();

                _log.Information("Vehicle Details added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Update(VehicleDetailDTO newDetail, string email)
        {
            try
            {
                VehicleDetail detail = _unitOfWork.VehiclesDetails.GetAll().Where(item => item.Driver.User.Email == email).FirstOrDefault();
                VehicleType vehicleCategory = _unitOfWork.VehicleCategories.Get(item => item.Type == newDetail.VehicleType);
                User user = _unitOfWork.Users.Get(item => item.Email == newDetail.UserEmail);

                if (detail == null) return false;

                if (detail.isDelete) return false;

                detail.Manufacturer = newDetail.Manufacturer;
                detail.RegistrationNumber = newDetail.RegistrationNumber;
                detail.RegistrationName = newDetail.RegistrationName;
                detail.Seating = newDetail.Seating;
                detail.VehicleTypeId = vehicleCategory.Id;
                detail.Seating = newDetail.Seating;
                detail.ModifiedAt = DateTime.Now;
                detail.ModifiedBy = user.Id;

                _unitOfWork.VehiclesDetails.Update(detail);
                _unitOfWork.Complete();

                _log.Information("Vehicle Details updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public VehicleDetailResponse Search(string email)
        {
            try
            {
                int vehicleId = (int)_unitOfWork.Drivers.GetAll().Where(item => item.User.Email == email).Select(item => item.VehicleId).FirstOrDefault();
                VehicleDetail vehicle = _unitOfWork.VehiclesDetails.Get(item => item.Id == vehicleId);
                VehicleType type = _unitOfWork.VehicleCategories.Get(item => item.Id == vehicle.VehicleTypeId);

                VehicleDetailResponse vehicleDetail = new()
                {
                    Manufacturer = vehicle.Manufacturer,
                    VehicleModel = vehicle.VehicleModel,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    RegistrationName = vehicle.RegistrationName,
                    Seating = vehicle.Seating,
                    VehicleType = type.Type
                };

                _log.Information("Vehicle detail fetched successfully");
                return vehicleDetail;
            }
            catch (Exception ex) { _log.Error(ex.StackTrace); return null; }
        }

        public bool Delete(int id)
        {
            try
            {
                VehicleDetail detail = _unitOfWork.VehiclesDetails.Get(item => item.Id == id);

                if (detail == null) return false;
                if (detail.isDelete) return false;

                detail.isDelete = true;

                _unitOfWork.VehiclesDetails.Update(detail);
                _unitOfWork.Complete();

                _log.Information("Vehicle detail deleted successfully");
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


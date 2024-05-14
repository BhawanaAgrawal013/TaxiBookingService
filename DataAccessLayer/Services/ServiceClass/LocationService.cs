namespace DataAccessLayer
{
    public class LocationService : ILocationService
    {
        private readonly ILocationUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public LocationService(ILocationUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<Location> Display()
        {
            try
            {
                _log.Information("Location: location list fetched successfully");
                return _unitOfWork.Locations.Display(item => item.isDelete == false);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        private int CreateLocation(LocationDTO locationDTO)
        {
            try
            {
                User user = _unitOfWork.Users.Get(item => item.Email == locationDTO.UserEmail);
                Area area = _unitOfWork.Areas.Get(item => item.Name == locationDTO.Area);

                if (area == null || area.isDelete == true) return 0;

                Location location = new()
                {
                    Lattitude = locationDTO.Latitude,
                    Longitude = locationDTO.Longitude,
                    Street = locationDTO.Street,
                    AreaId = area.Id,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.Locations.Add(location);
                _unitOfWork.Complete();
                return location.Id;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }
        }

        public bool Add(LocationDTO locationDTO)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == locationDTO.UserEmail)) return false;

                Driver driver = _unitOfWork.Drivers.GetAll().Where(item => item.User.Email == locationDTO.UserEmail).FirstOrDefault();

                if (driver.LocationId != null)
                {
                    Update(locationDTO, locationDTO.UserEmail);
                    return true;
                }

               int locationId = CreateLocation(locationDTO);

                if (locationId == 0) return false;

                driver.LocationId = locationId;

                _unitOfWork.Drivers.Update(driver);
                _unitOfWork.Complete();

                _log.Information("Location for driver added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public LocationDTO Search(string email)
        {
            try
            {
                Location location = _unitOfWork.Locations.GetAll().Where(item => item.Driver.User.Email ==  email).FirstOrDefault();
                LocationDTO locationDTO = new()
                {
                    Latitude = location.Lattitude,
                    Longitude = location.Longitude,
                    Street = location.Street,
                    Area = location.Area.Name
                };

                _log.Information("Location searched successfully");
                return locationDTO;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(LocationDTO locationDTO, string email)
        {
            try
            {
                Location location = _unitOfWork.Locations.Get(item => item.Driver.User.Email == email);
                Area area = _unitOfWork.Areas.Get(item => item.Name == locationDTO.Area);
                User user = _unitOfWork.Users.Get(item => item.Email == locationDTO.UserEmail);

                if (location == null || location.isDelete) return false;

                location.Lattitude = locationDTO.Latitude;
                location.Longitude = locationDTO.Longitude;
                location.Street = locationDTO.Street;
                location.AreaId = area.Id;
                location.ModifiedAt = DateTime.Now;
                location.ModifiedBy = user.Id;

                _unitOfWork.Locations.Update(location);
                _unitOfWork.Complete();

                _log.Information("Location for driver updated successfully");

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
                Location location = _unitOfWork.Locations.Get(item => item.Id == id);

                if (location == null) return false;
                if (location.isDelete) return false;

                location.isDelete = true;

                _unitOfWork.Locations.Delete(location);
                _unitOfWork.Complete();

                _log.Information("Location for driver deleted successfully");

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

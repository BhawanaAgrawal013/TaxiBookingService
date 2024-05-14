namespace DataAccessLayer
{
    public class DriverService : IDriverService
    {
        private readonly IDriverUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRatingService _ratingService;
        private readonly IHashingService _hashingService;
        private readonly ILog _log;

        public DriverService(IDriverUnitOfWork unitOfWork, IUserService userService, IHashingService hashingService, ILog log, IRatingService ratingService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _hashingService = hashingService;
            _log = log;
            _ratingService = ratingService;
        }

        public bool Add(DriverDTO newDriver)
        {
            try
            {
                if (_unitOfWork.Users.Exists(item => item.Email == newDriver.Email))
                {
                    User existingUser = _unitOfWork.Users.Get(item => item.Email == newDriver.Email);
                    if (existingUser.isDelete == false) return false;
                }

                if (AddUser(newDriver))
                {
                    int userId = (int)_unitOfWork.Users.Get(e => e.Email == newDriver.Email).Id;
                    if (userId == 0) { return false; }

                    Driver driver = new()
                    {
                        DrivingLicense = newDriver.DrivingLicense,
                        UserId = userId,
                        isApproved = false,
                        CreatedAt = DateTime.Now,
                        isDelete = false,
                        CreatedBy = userId
                    };

                    _unitOfWork.Drivers.Add(driver);
                    _unitOfWork.Complete();

                    _log.Information("DriverRepo: Driver has been added succesfully");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool AddUser(DriverDTO userdto)
        {
            try
            {
                UserDTO user = new UserDTO();
                user.FirstName = userdto.FirstName;
                user.LastName = userdto.LastName;
                user.DateOfBirth = userdto.DateOfBirth;
                user.Email = userdto.Email;
                user.Gender = userdto.Gender;
                user.PhoneNumber = userdto.PhoneNumber;
                user.Password = userdto.Password;
                user.RoleName = Roles.Driver.ToString();

                if (_userService.Add(user))
                {
                    _log.Information("User for driver created successfully");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public List<AvailableVehicleDTO> GetAvailableVehicles(decimal latitude, decimal longitude)
        {
            try
            {
                List<AvailableVehicleDTO> vehicles = new List<AvailableVehicleDTO>();
                List<int> locationIds = new List<int>();

                foreach (var driver in _unitOfWork.Locations.FindAll(e => e.isDelete == false))
                {
                    if (ApproximatelyEquals(latitude, driver.Lattitude, 1.1111) && ApproximatelyEquals(longitude, driver.Longitude, 1.1111))
                    {
                        locationIds.Add(driver.Id);
                    }
                }

                foreach (var locationId in locationIds)
                {
                    var drivers = _unitOfWork.Drivers.GetAll().Find(e => e.LocationId == locationId);
                    
                    if (drivers.VehicleId == null) continue;

                    AvailableVehicleDTO vehicle = new()
                    {
                        Gender = drivers.User.Gender,
                        Name = drivers.User.FirstName + drivers.User.LastName,
                        Manufacturer = drivers.VehicleDetail.Manufacturer,
                        Seating = drivers.VehicleDetail.Seating,
                        VehicleModel = drivers.VehicleDetail.VehicleModel,
                        Email = drivers.User.Email,
                        VehicleType = drivers.VehicleDetail.VehicleType.Type,
                        Rating = drivers.User.AverageRating //_ratingService.CalculateRating(drivers.Id),
                    };
                    vehicles.Add(vehicle);
                }

                _log.Information("DriverRepo: Get available vehicles fetched");
                return vehicles;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        private bool ApproximatelyEquals(decimal value1, decimal value2, double acceptableDifference)
        {
            double val1 = (double)value1;
            double val2 = (double)value2;
            return Math.Abs(val1 - val2) <= acceptableDifference;
        }

        public AvailableVehicleDTO CurrentDriver(string email)
        {
            try
            {
                Driver driver = _unitOfWork.Drivers.GetAll().Where(e => e.User.Email == email && e.isDelete == false).FirstOrDefault();

                AvailableVehicleDTO availableVehicleDTO = new()
                {
                    Gender = driver.User.Gender,
                    Name = driver.User.FirstName + driver.User.LastName,
                    Manufacturer = driver.VehicleDetail.Manufacturer,
                    Seating = driver.VehicleDetail.Seating,
                    VehicleModel = driver.VehicleDetail.VehicleModel,
                    VehicleType = driver.VehicleDetail.VehicleType.Type,
                    Rating = _ratingService.CalculateRating(driver.Id),
                    RegistrationNumber = driver.VehicleDetail.RegistrationNumber
                };

                _log.Information("DriverRepo: Current driver fetched");
                return availableVehicleDTO;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public List<DriverDTO> Display()
        {
            try
            {
                var drivers = _unitOfWork.Drivers.GetAll().Where(item => item.isApproved == false);

                List<DriverDTO> driverDTOs = new List<DriverDTO>();

                foreach (var driver in drivers)
                {
                    DriverDTO driverDTO = new()
                    {
                        FirstName = driver.User.FirstName,
                        LastName = driver.User.LastName,
                        DateOfBirth = driver.User.DateOfBirth,
                        Email = driver.User.Email,
                        Gender = driver.User.Gender,
                        PhoneNumber = driver.User.PhoneNumber,
                        RoleName = driver.User.Role.RoleName,
                        DrivingLicense = driver.DrivingLicense,
                        Rating = _ratingService.CalculateRating(driver.Id)
                    };
                    driverDTOs.Add(driverDTO);
                }

                _log.Information("DriverRepo: Driver displayed successfully");
                return driverDTOs;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public DriverDTO Search(string email)
        {
            try
            {
                Driver driver = _unitOfWork.Drivers.GetAll().Where(e => e.User.Email == email && e.isDelete == false).FirstOrDefault();

                DriverDTO driverDTO = new()
                {
                    FirstName = driver.User.FirstName,
                    LastName = driver.User.LastName,
                    DateOfBirth = driver.User.DateOfBirth,
                    Email = driver.User.Email,
                    Gender = driver.User.Gender,
                    PhoneNumber = driver.User.PhoneNumber,
                    RoleName = driver.User.Role.RoleName,
                    DrivingLicense = driver.DrivingLicense,
                    Wallet = driver.User.wallet,
                    Rating =  driver.User.AverageRating //_ratingService.CalculateRating(driver.Id)
                };

                _log.Information("DriverRepo: Driver searched clearly");
                return driverDTO;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool UpdateUser(DriverEditModel user)
        {
            try
            {
                UserEditModel userdto = new()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                };
                if(_userService.Update(userdto, user.Email)) 
                {
                    return true;
                }

                _log.Information("DriverRepo: user for driver updated successfully");
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Update(DriverEditModel userdto, string email)
        {
            try
            {
                Driver driver = _unitOfWork.Drivers.Get(e => e.User.Email == email && e.isDelete == false);

                if (driver == null) return false;

                if (driver.isDelete == true) return false;

                if (UpdateUser(userdto))
                {
                    Driver driverUser = new Driver();
                    driverUser.UserId = driver.UserId;
                    driverUser.DrivingLicense = userdto.DrivingLicense;
                    driverUser.ModifiedBy = driver.UserId;

                    if (driverUser.ModifiedBy == 0) return false;

                    driverUser.ModifiedAt = DateTime.Now;

                    _unitOfWork.Drivers.Update(driverUser);
                    _unitOfWork.Complete();

                    _log.Information("Driver updated successfully");
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Delete(int id)
        {
            Driver driver = _unitOfWork.Drivers.GetAll().Find(e => e.Id == id);
            try
            {
                driver.isDelete = true;
                driver.User.isDelete = true;
                _unitOfWork.Complete();

                _log.Information("DriverRepo: driver deleted successfully");
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
            try
            {
                User user = _unitOfWork.Users.Get(item => item.Email == email);

                if (user == null) return false;

                string role = _unitOfWork.UserRoles.GetRoleName(user.RoleId);

                if (!(role == Roles.Driver.ToString())) return false;

                bool isValidPassword = _hashingService.VerifyHashing(password, user.Password);

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
        public List<BookingRequestDTO> GetBookingRequests()
        {
            try
            {
                List<BookingRequestDTO> bookings = new List<BookingRequestDTO>();

                var booking = _unitOfWork.Bookings.GetAll().FindAll(e => e.PickupDate >= DateTime.Now);
                bookings = booking.FindAll(e => e.StatusId == (int)Statuses.Requested).Select(e => new BookingRequestDTO { Id = e.Id, DropLocation = e.DropoffLocation, PickupLocation = e.PickupLocation, Name = e.User.FirstName + e.User.LastName, Payment = e.Payment.amount, PhoneNumber = e.PhoneNumber, PickupTime = e.PickupDate }).ToList();

                if (bookings.Count == 0) return null;

                _log.Information("DriverRepo: driver booking request fetched successfully");
                return bookings;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public BookingRequestDTO GetingBookingRequest(string email)
        {
            try
            {
                int driverId = _unitOfWork.Drivers.GetAll().Find(e => e.User.Email == email).Id;
                var booking = _unitOfWork.Bookings.GetAll().FindLast(e => e.DriverId == driverId && e.StatusId == 1);

                if (booking == null) return null;

                BookingRequestDTO bookingRequest = new()
                {
                    Id = booking.Id,
                    DropLocation = booking.DropoffLocation,
                    PickupLocation = booking.PickupLocation,
                    Name = booking.User.FirstName + booking.User.LastName,
                    Payment = booking.Payment.amount,
                    PhoneNumber = booking.PhoneNumber,
                    PickupTime = booking.PickupDate,
                };

                return bookingRequest;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool ApproveDriver(string email)
        {
            try
            {
                if (!_unitOfWork.Drivers.Exists(item => item.User.Email == email)) return false;

                Driver driver = _unitOfWork.Drivers.GetAll().Where(item => item.User.Email == email).FirstOrDefault();
                driver.isApproved = true;

                _unitOfWork.Drivers.Update(driver);
                _unitOfWork.Complete();

                _log.Information("DriverRepo: driver approved by admin");
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

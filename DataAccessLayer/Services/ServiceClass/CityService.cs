namespace DataAccessLayer
{
    public class CityService : ICityService
    {
        private readonly ICityUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public CityService(ICityUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<CityResponse> Display()
        {
            try
            {
                var cities = _unitOfWork.Cities.GetCities();
            
                List<CityResponse> cityDMs = new List<CityResponse>();
                foreach (var city in cities)
                {
                    CityResponse cityDM = new()
                    {
                        Id = city.Id,
                        City = city.Name,
                        State = city.State.Name
                    };
                    cityDMs.Add(cityDM);
                }

                _log.Information("CityRepo: City list fetched successfully");
                return cityDMs;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(CityDTO cityDTO)
        {
            try
            {
                if (!_unitOfWork.Users.Exists(item => item.Email == cityDTO.UserEmail)) return false;

                if (_unitOfWork.Cities.Exists(item => item.Name == cityDTO.City))
                {
                    City existingCity = _unitOfWork.Cities.Get(item => item.Name == cityDTO.City);

                    if (existingCity.isDelete == false || existingCity == null) return false;
                }
                User user = _unitOfWork.Users.Get(item => item.Email == cityDTO.UserEmail);
                State state = _unitOfWork.States.Get(item => item.Name == cityDTO.State);

                City city = new()
                {
                    Name = cityDTO.City,
                    StateId = state.Id,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.Cities.Add(city);
                _unitOfWork.Complete();

                _log.Information("CityRepo: City was added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public CityResponse Search(int id)
        {
            try
            {
                _log.Information("CityRepo: City searched successfully");
                return _unitOfWork.Cities.GetCities().Where(item => item.isDelete == false && item.Id == id).Select(e => new CityResponse { State = e.State.Name, Id = e.Id, City = e.Name }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(CityDTO cityDTO, int id)
        {
            try
            {
                City city = _unitOfWork.Cities.Get(item => item.Id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == cityDTO.UserEmail);

                if (city == null || city.isDelete) return false;

                city.Name = cityDTO.City;
                city.ModifiedAt = DateTime.Now;
                city.ModifiedBy = user.Id;

                _unitOfWork.Cities.Update(city);
                _unitOfWork.Complete();

                _log.Information("CityRepo: city was updated successfully");
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
                City city = _unitOfWork.Cities.Get(item => item.Id == id);

                if (city == null || city.isDelete) return false;
                city.isDelete = true;

                _unitOfWork.Cities.Update(city);
                _unitOfWork.Complete();

                _log.Information("CityRepo: city was deleted successfully");
                return true;
            }
            catch (Exception ex) { _log.Error(ex.StackTrace); return false; }
        }
    }
}

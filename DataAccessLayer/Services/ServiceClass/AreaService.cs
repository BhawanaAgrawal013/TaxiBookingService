using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class AreaService : IAreaService
    {
        private readonly IAreaUnitOfWork _unitOfWork;
        private readonly ILog _log;

        public AreaService(IAreaUnitOfWork unitOfWork, ILog log)
        {
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public List<AreaResponse> Display()
        {
            try
            {
                var areas = _unitOfWork.Areas.GetAll();

                List<AreaResponse> areaDMs = new List<AreaResponse>();

                foreach (var area in areas)
                {
                    AreaResponse areaDM = new AreaResponse();
                    areaDM.Area = area.Name;
                    areaDM.City = area.City.Name;
                    areaDM.Pincode = area.Pincode;
                    areaDM.State = area.City.State.Name;
                    areaDM.Id = area.Id;

                    areaDMs.Add(areaDM);
                }

                _log.Information("Areas returned successfully");
                return areaDMs;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Add(AreaDTO areadto)
        {
            try
            {
                if (_unitOfWork.Areas.Exists(item => item.Pincode == areadto.Pincode))
                {
                    Area existingArea = _unitOfWork.Areas.Get(item => item.Pincode == areadto.Pincode);

                    if (existingArea.isDelete == false || existingArea == null)
                    {
                        _log.Error("User is deleted");
                        return false;
                    }
                }

                User user = _unitOfWork.Users.Get(item => item.Email == areadto.UserEmail);

                if(user == null) return false;

                City city = _unitOfWork.Cities.Get(item => item.Name == areadto.City);

                Area area = new()
                {
                    Name = areadto.Area,
                    Pincode = areadto.Pincode,
                    CityId = city.Id,
                    CreatedAt = DateTime.Now,
                    isDelete = false,
                    CreatedBy = (int)user.Id
                };

                _unitOfWork.Areas.Add(area);
                _unitOfWork.Complete();

                _log.Information("Area added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public AreaResponse Search(int id)
        {
            try
            {
                if (!_unitOfWork.Areas.Exists(item => item.Id == id)) return null;

                Area area = _unitOfWork.Areas.GetAll().Find(item => item.Id == id);

                AreaResponse areaDM = new AreaResponse();
                areaDM.Area = area.Name;
                areaDM.City = area.City.Name;
                areaDM.Pincode = area.Pincode;
                areaDM.State = area.City.State.Name;
                areaDM.Id = area.Id;

                _log.Information("Area fetched successfully");
                return areaDM;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public bool Update(AreaDTO areadto, int id)
        {
            try
            {
                if (!_unitOfWork.Areas.Exists(item => item.Id == id) || !_unitOfWork.Cities.Exists(item => item.Name == areadto.City)) return false;

                Area area = _unitOfWork.Areas.Get(item => item.Id == id);
                User user = _unitOfWork.Users.Get(item => item.Email == areadto.UserEmail);

                if (area == null || area.isDelete) return false;

                area.Name = areadto.Area;
                area.CityId = _unitOfWork.Cities.Find((item => item.Name == areadto.City)).Id;
                area.Pincode = areadto.Pincode;
                area.ModifiedAt = DateTime.Now;
                area.ModifiedBy = user.Id;

                _unitOfWork.Areas.Update(area);
                _unitOfWork.Complete();

                _log.Information("Area updated successfully");
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
                Area area = _unitOfWork.Areas.Get(item => item.Id == id);

                if (area == null) return false;
                if (area.isDelete) return false;

                area.isDelete = true;

                _unitOfWork.Areas.Update(area);
                _unitOfWork.Complete();

                _log.Information("Area deleted successfully");
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

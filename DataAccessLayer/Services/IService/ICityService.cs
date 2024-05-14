namespace DataAccessLayer
{
    public interface ICityService
    {
        List<CityResponse> Display();
        bool Add(CityDTO newCity);
        CityResponse Search(int id);
        bool Update(CityDTO updatedCity, int id);
        bool Delete(int id);
    }
}

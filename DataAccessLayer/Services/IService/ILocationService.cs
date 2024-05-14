namespace DataAccessLayer
{
    public interface ILocationService
    {
        List<Location> Display();
        bool Add(LocationDTO newLocation);
        LocationDTO Search(string email);
        bool Update(LocationDTO updatedLocation, string email);
        bool Delete(int id);
    }
}

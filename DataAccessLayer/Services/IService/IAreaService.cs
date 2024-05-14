namespace DataAccessLayer
{
    public interface IAreaService
    {
        List<AreaResponse> Display();
        bool Add(AreaDTO newArea);
        AreaResponse Search(int id);
        bool Update(AreaDTO updatedArea, int id);
        bool Delete(int id);
    }
}

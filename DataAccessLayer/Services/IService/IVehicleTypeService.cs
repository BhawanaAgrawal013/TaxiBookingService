namespace DataAccessLayer
{
    public interface IVehicleTypeService
    {
        List<VehicleTypeResponse> Display();
        bool Add(VehicleTypeDTO newCategory);

        VehicleTypeDTO Search(int id);
        bool Update(VehicleTypeDTO updatedCategory, int id);
        bool Delete(int id);
    }
}

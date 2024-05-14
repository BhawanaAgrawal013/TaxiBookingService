namespace DataAccessLayer
{
    public interface IVehicleDetailsService
    {
        List<VehicleDetailResponse> Display();
        bool Add(VehicleDetailDTO newDetail);
        VehicleDetailResponse Search(string userEmail);
        bool Update(VehicleDetailDTO updatedDetail, string email);
        bool Delete(int id);
    }
}

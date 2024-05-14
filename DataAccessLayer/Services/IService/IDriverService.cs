namespace DataAccessLayer
{
    public interface IDriverService
    {
        public List<DriverDTO> Display();
        public DriverDTO Search(string email);
        public AvailableVehicleDTO CurrentDriver(string email);
        public bool Add(DriverDTO driverdto);
        public bool Update(DriverEditModel driverdto, string email);
        public bool Delete(int id);
        public bool Login(string email, string password);
        public List<AvailableVehicleDTO> GetAvailableVehicles(decimal latitude, decimal longitude);
        public List<BookingRequestDTO> GetBookingRequests();
        public BookingRequestDTO GetingBookingRequest(string email);
        public bool ApproveDriver(string email);
    }
}

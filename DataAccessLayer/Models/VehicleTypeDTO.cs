namespace DataAccessLayer.Models
{
    public class VehicleTypeDTO
    {
        [Required]
        [StringLength(15)]
        public string VehicleType { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}

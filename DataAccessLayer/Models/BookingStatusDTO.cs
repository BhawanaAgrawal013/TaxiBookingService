namespace DataAccessLayer.Models
{
    public class BookingStatusDTO
    {
        [Required]
        [StringLength(15, ErrorMessage = "Booking status cannot be more than 15")]
        public string Status { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string UserEmail { get; set; }
    }
}

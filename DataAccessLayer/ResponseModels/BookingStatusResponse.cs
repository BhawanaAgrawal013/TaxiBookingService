namespace DataAccessLayer
{
    public class BookingStatusResponse
    {
        public int Id {  get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Booking Status cannot be more than 15")]
        public string Status { get; set; }
    }
}

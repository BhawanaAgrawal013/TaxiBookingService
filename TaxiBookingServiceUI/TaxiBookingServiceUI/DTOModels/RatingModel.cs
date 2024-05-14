namespace TaxiBookingServiceUI.DTOModels
{
    public class RatingModel
    {
        [Required]
        public decimal Ratings { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Comment should be under 200 characters")]
        public string Comment { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int BookingId { get; set; }
    }
}

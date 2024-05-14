using System.ComponentModel.DataAnnotations;

namespace TaxiBookingServiceUI.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(20, ErrorMessage = "Email cannot be more than 20")]
        public string Email { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

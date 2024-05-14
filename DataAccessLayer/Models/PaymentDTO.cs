using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataAccessLayer.Models
{
    public class PaymentDTO
    {
        [Required]
        [DataType(DataType.Currency)]
        [Range(1, int.MaxValue)]
        [DisplayName("Booking Amount")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(15)]
        public string Mode { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}

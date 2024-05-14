using System.ComponentModel.DataAnnotations;

namespace TaxiBookingServiceUI.Models
{
    public class StateViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Name")]
        public string State { get; set; }
    }
}

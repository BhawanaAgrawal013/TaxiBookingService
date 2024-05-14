using System.ComponentModel.DataAnnotations;

namespace TaxiBookingServiceUI.Models
{
    public class AreaViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Area Name")]
        public string Area { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid City Name")]
        public string City { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid State Name")]
        public string State { get; set; }


        [Required]
        [StringLength(6)]
        [DataType(DataType.PostalCode)]
        public string Pincode { get; set; }
    }
}

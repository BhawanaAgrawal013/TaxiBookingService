using System.ComponentModel.DataAnnotations;

namespace TaxiBookingServiceUI.Models
{
    public class LocationViewModel
    {

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Street Name")]
        public string Street { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Area Name")]
        public string Area { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Street Name")]
        public string City { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Area Name")]
        public string State { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string UserEmail { get; set; }
    }
}

﻿namespace DataAccessLayer.Models
{
    public class AreaDTO
    {
        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid Area Name")]
        public string Area { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid City Name")]
        public string City { get; set; }

        [Required]
        [StringLength(6)]
        [DataType(DataType.PostalCode)]
        public string Pincode { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string UserEmail { get; set; }
    }
}

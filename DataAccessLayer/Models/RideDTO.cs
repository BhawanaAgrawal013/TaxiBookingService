using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class RideDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string PickupLocation { get; set; }

        [Required]
        public DateTime PickupTime { get; set; }

        [Required]
        public DateTime DropTime { get; set; }

        [Required]
        public string DropLocation { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Email cannot be more than 20")]
        public string UserEmail { get; set; }

        [Required]
        public double Payment { get; set; }

        [Required]
        public double TotalTime { get; set; }

        [Required]
        public string PaymentMode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}

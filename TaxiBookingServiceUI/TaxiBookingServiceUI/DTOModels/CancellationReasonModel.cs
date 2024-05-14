namespace TaxiBookingServiceUI.DTOModels
{
    public class CancellationReasonModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Cancellation reason should be less than 50")]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid reason")]
        public string Reason { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        public string UserEmail { get; set; }

        public bool IsCharged { get; set; }
    }
}

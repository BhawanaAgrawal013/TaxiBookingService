namespace DataAccessLayer.Models
{
    public class CancellationReasonDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Cancellation reason should be less than 50")]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid reason")]
        public string Reason { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        public bool IsCharged { get; set; }
    }
}

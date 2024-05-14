namespace DataAccessLayer
{
    public class StateResponse
    {
        public int Id {  get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-za-z]*((-|\s)*[A-Za-z])*$", ErrorMessage = "Not a valid State")]
        public string State { get; set; }
    }
}

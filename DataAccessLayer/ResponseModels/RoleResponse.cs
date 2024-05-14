namespace DataAccessLayer
{
    public class RoleResponse
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "String cannot be more than 10")]
        public string RoleName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
    public class RoleDTO
    {
        [Required]
        [StringLength(10, ErrorMessage = "String cannot be more than 10")]
        public string RoleName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email cannot be more than 30")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
    }
}

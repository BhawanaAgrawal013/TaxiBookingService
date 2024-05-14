using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entity
{
    [NotMapped]
    public class CommonProperties
    {
        [Column(TypeName = "bit")]
        public bool isDelete { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "int")]
        public int? CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ModifiedAt { get; set; }

        [Column(TypeName = "int")]
        public int? ModifiedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User Created { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User Modified { get; set; }
    }
}

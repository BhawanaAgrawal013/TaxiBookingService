namespace DataAccessLayer.Entity
{
    public class VehicleDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Manufacturer { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string VehicleModel { get; set; }

        [Column(TypeName = "int")]
        public int VehicleTypeId { get; set; }

        [Column(TypeName = "int")]
        public int Seating { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string RegistrationNumber { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string RegistrationName { get; set; }

        [Column(TypeName = "bit")]
        public bool isApproved { get; set; }

        [Column(TypeName = "bit")]
        public bool isDelete { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "int")]
        public int CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedAt { get; set; }

        [Column(TypeName = "int")]
        public int? ModifiedBy { get; set; }

        public virtual VehicleType VehicleType { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User Created { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User Modified { get; set; }
        public virtual Driver Driver { get; set; }
    }
}

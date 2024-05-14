namespace DataAccessLayer.Entity
{
    public class Driver
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "int")]
        public int UserId { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string DrivingLicense { get; set; }

        [Column(TypeName = "int")]
        public int? VehicleId { get; set; }

        [Column(TypeName = "bit")]
        public bool isApproved { get; set; }

        [Column(TypeName = "int")]
        public int? LocationId { get; set; }

        [Column(TypeName = "bit")]
        public bool isDelete { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "int")]
        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedAt { get; set; }

        [Column(TypeName = "int")]
        public int? ModifiedBy { get; set; }

        /*[ForeignKey("CreatedBy")]*/
        public virtual User Created { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User Modified { get; set; }
        public virtual VehicleDetail VehicleDetail { get; set; }
        public virtual Rating Ratings { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Location Location { get; set; }
    }
}

namespace DataAccessLayer.Entity
{
    public class Rating 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Ratings { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Comment { get; set; }

        [Column(TypeName = "int")]
        public int? UserId { get; set; }

        [Column(TypeName = "int")]
        public int? DriverId { get; set; }

        [Column(TypeName = "bit")]
        public bool isDelete { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "int")]
        //[ForeignKey("Created")]
        public int CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedAt { get; set; }

        [Column(TypeName = "int")]
        public int? ModifiedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User Created { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User Modified { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual User User { get; set; }

        /*    [ForeignKey("UserId")]*/
    }
}

namespace DataAccessLayer.Entity
{
    public class CancellationReason
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Reason { get; set; }

        [Column(TypeName = "bit")]
        public bool isCharged { get; set; }

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

        [ForeignKey("CreatedBy")]
        public virtual User Created { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User Modified { get; set; }
        public virtual ICollection<CancelledBooking> CancelledBookings { get; set; }

    }
}

namespace DataAccessLayer.Entity
{
    public class CancelledBooking 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "int")]
        public int BookingId { get; set; }

        [Column(TypeName = "int")]
        public int ReasonId { get; set; }

        [Column(TypeName = "bit")]
        public bool isPending { get; set; }

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
        public virtual Booking Booking { get; set; }

        [ForeignKey("ReasonId")]
        public virtual CancellationReason CancellationReason { get; set;}
    }
}

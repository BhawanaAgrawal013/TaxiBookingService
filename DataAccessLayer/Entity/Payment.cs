namespace DataAccessLayer.Entity
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        [Column(TypeName = "int")]
        public int ModeId { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string Status { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Remark { get; set; }

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

        [ForeignKey("ModeId")]
        public virtual PaymentMode PaymentMode { get; set; }
        public virtual Booking Booking { get; set; }
    }
}

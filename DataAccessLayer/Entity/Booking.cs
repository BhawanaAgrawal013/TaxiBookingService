namespace DataAccessLayer.Entity
{
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string PickupLocation { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string DropoffLocation { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "int")]
        public int StatusId { get; set; }

        [Column(TypeName = "int")]
        public int? DriverId { get; set; }

        [Column(TypeName = "int")]
        public int? UserId { get; set; }

        [Column(TypeName = "int")]
        public int? PaymentId { get; set; }

        [Column(TypeName = "int")]
        public int? RatingId { get; set; }

        [Column(TypeName = "int")]
        [ForeignKey("UserRating")]
        public int? UserRatingId { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string VehiclePreference { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime PickupDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DropOffTime { get; set; }

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
        public virtual User User { get; set; }
        public virtual Driver Driver { get; set; }

        [ForeignKey("StatusId")]
        public virtual BookingStatus BookingStatus { get; set; }

        public virtual User Created { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User Modified { get; set; }

        [ForeignKey("RatingId")]
        public virtual Rating Rating { get; set; }

        public virtual Rating UserRating { get; set; }
        public virtual Payment Payment { get; set; }
    }
}

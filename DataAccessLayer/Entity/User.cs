namespace DataAccessLayer.Entity
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(6)")]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string Password { get; set; }

        [Column(TypeName = "int")]
        public int RoleId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal wallet { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal AverageRating { get; set; }

        [Column(TypeName = "bit")]
        public bool isDelete { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "int")]
        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedAt { get; set; }

        [Column(TypeName = "int")]
        public int? ModifiedBy { get; set; }

        public virtual Role Role { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Driver CreatedDriver { get; set; }
        public virtual Rating CreatedRating { get;}

        [ForeignKey("CreatedBy")]
        public virtual User Created { get; set; }

        [ForeignKey("ModifiedBy")]
        public virtual User Modified { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}

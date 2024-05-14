namespace DataAccessLayer.Entity
{
    public class Location 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string Street { get; set; }

        [Column(TypeName = "int")]
        public int AreaId { get; set; }

        [Column(TypeName = "varchar(11)")]
        public decimal Longitude { get; set; }

        [Column(TypeName = "varchar(11)")]
        public decimal Lattitude { get; set; }

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
        public virtual Area Area { get; set; }
        public virtual Driver Driver { get; set; }
    }
}

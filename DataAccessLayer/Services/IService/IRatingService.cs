namespace DataAccessLayer
{
    public interface IRatingService
    {
        public decimal CalculateRating(int id);
        public bool Add(RatingDTO ratingdto);
        public bool UserRating(RatingDTO ratingdto);
        public bool Update(RatingDTO ratingdto, int id);
    }
}

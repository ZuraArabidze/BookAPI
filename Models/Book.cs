using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [Range(1000, 2025)]
        public int PublicationYear { get; set; }

        [Required]
        [MaxLength(100)]
        public string AuthorName { get; set; }

        public int ViewsCount { get; set; } = 0;

        public double CalculatePopularityScore()
        {
            int yearsSincePublished = DateTime.Now.Year - PublicationYear;
            return (ViewsCount * 0.5) + (yearsSincePublished * 2);
        }
    }
}

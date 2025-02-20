using System.ComponentModel.DataAnnotations;

namespace BookAPI.Dtos
{
    public class BookCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Publication year is required")]
        [Range(1000, 2025, ErrorMessage = "Publication year must be between 1000 and 2025")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [MaxLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        public string AuthorName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BookAPI.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BookAPI.Dtos
{
    public class UserLogInDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

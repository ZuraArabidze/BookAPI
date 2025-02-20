using System.ComponentModel.DataAnnotations;

namespace BookAPI.Dtos
{
    public class BulkDeleteDto
    {
        [Required]
        public List<int> BookIds { get; set; }
    }
}

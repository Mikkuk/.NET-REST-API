


using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(300, ErrorMessage = "Title must be at most 300 characters long")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(300, ErrorMessage = "Content must be at most 300 characters long")]
        public string Content { get; set; } = string.Empty;
    }
}
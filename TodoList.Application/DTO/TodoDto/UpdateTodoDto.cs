

using System.ComponentModel.DataAnnotations;

namespace TodoList.Application.DTO
{
    public class UpdateTodoDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}

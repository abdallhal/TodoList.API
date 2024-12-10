
namespace TodoList.Application.DTO
{
    public class CreateTodoDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}

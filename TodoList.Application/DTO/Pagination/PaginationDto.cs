
namespace TodoList.Application.DTO
{
    public class PaginationDto
    {
        public int PageIndex { get; set; } = 0; 
        public int PageSize { get; set; } = 10; 
        public int TotalRecords { get; set; } = 0;  
    }
}

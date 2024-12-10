using Microsoft.AspNetCore.Mvc;
using TodoList.Application.DTO;
using TodoList.Application.Interfaces;
using TodoList.Presentation.Routes;

[ApiController]
[Route(TodoRoutes.BaseRoute)]  
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

  
    [HttpGet(TodoRoutes.GetAll)]
    public IActionResult GetAll([FromQuery] GetTodoSearchDTO getTodoSearchDTO, [FromQuery] PaginationDto paginationDto)
    {
        var response =  _todoService.GetAll(getTodoSearchDTO, paginationDto);

        if (response.IsSuccess)
        {
            return Ok(response); 
        }

        return StatusCode(response.StatusCode, response); 
    }


    [HttpGet(TodoRoutes.GetAllPending)]
    public IActionResult GetAllPending([FromQuery] GetTodoSearchDTO getTodoSearchDTO, [FromQuery] PaginationDto paginationDto)
    {
        var response =  _todoService.GetAllPending(getTodoSearchDTO, paginationDto);

        if (response.IsSuccess)
        {
            return Ok(response); 
        }

        return StatusCode(response.StatusCode, response); 
    }

    
    [HttpPost(TodoRoutes.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto dto)
    {
        var response = await _todoService.CreateAsync(dto);

        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetAll), new { id = response.Data.Id }, response);
        }

        return StatusCode(response.StatusCode, response); 
    }

    
    [HttpPut(TodoRoutes.Update)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoDto dto)
    {
        var response = await _todoService.UpdateAsync(id, dto);

        if (response.IsSuccess)
        {
            return Ok(response); 
        }

        return StatusCode(response.StatusCode, response); 
    }

    [HttpPatch(TodoRoutes.Complete)]
    public async Task<IActionResult> Complete(int id)
    {
        var response = await _todoService.CompleteAsync(id);

        if (response.IsSuccess)
        {
            return Ok(response); 
        }

        return StatusCode(response.StatusCode, response); 
    }


    [HttpDelete(TodoRoutes.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _todoService.DeleteAsync(id);

        if (response.IsSuccess)
        {
            return NoContent(); 
        }

        return StatusCode(response.StatusCode, response); 
    }
}

using AutoMapper;
using Microsoft.Extensions.Logging;
using TodoList.Application.DTO;
using TodoList.Application.Interfaces;
using TodoList.Domain.Common;
using TodoList.Domain.Common.PageList;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;


namespace TodoList.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoService> _logger;

        public TodoService(ITodoRepository repository, IMapper mapper, ILogger<TodoService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public BaseResponse<IPagedList<TodoItem>> GetAll(GetTodoSearchDTO getTodoSearchDTO, PaginationDto paginationDto)
        {
            try
            {
                _logger.LogInformation("Fetching all Todo items with search parameters {@SearchDTO}", getTodoSearchDTO);

                var query = _repository.GetAll();
                query = ApplyTodoSearch(query, getTodoSearchDTO);

                var pagedData = new PagedList<TodoItem>(query, paginationDto.PageIndex, paginationDto.PageSize);
                paginationDto.TotalRecords = pagedData.TotalCount;

                _logger.LogInformation("Successfully fetched {TotalCount} Todo items.", pagedData.TotalCount);

                return BaseResponse<IPagedList<TodoItem>>.SuccessResponse(pagedData, "Data retrieved successfully.", paginationDto.TotalRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Todo items.");
                return BaseResponse<IPagedList<TodoItem>>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        public BaseResponse<IPagedList<TodoItem>> GetAllPending(GetTodoSearchDTO getTodoSearchDTO, PaginationDto paginationDto)
        {
            try
            {
                _logger.LogInformation("Fetching all pending Todo items with search parameters {@SearchDTO}", getTodoSearchDTO);

                var query = _repository.GetAll(t => !t.IsCompleted);
                query = ApplyTodoSearch(query, getTodoSearchDTO);

                var pagedData = new PagedList<TodoItem>(query, paginationDto.PageIndex, paginationDto.PageSize);
                paginationDto.TotalRecords = pagedData.TotalCount;

                _logger.LogInformation("Successfully fetched {TotalCount} pending Todo items.", pagedData.TotalCount);

                return BaseResponse<IPagedList<TodoItem>>.SuccessResponse(pagedData, "Pending data retrieved successfully.", paginationDto.TotalRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching pending Todo items.");
                return BaseResponse<IPagedList<TodoItem>>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        private IQueryable<TodoItem> ApplyTodoSearch(IQueryable<TodoItem> query, GetTodoSearchDTO getTodoSearchDTO)
        {
            if (!string.IsNullOrEmpty(getTodoSearchDTO.Title))
            {
                _logger.LogInformation("Filtering Todo items with title containing {Title}", getTodoSearchDTO.Title);
                query = query.Where(todo => todo.Title.ToLower().Contains(getTodoSearchDTO.Title.ToLower()));
            }

            return query;
        }

        public async Task<BaseResponse<TodoItem>> CreateAsync(CreateTodoDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    _logger.LogWarning("Attempt to create Todo item with empty title.");
                    return BaseResponse<TodoItem>.FailureResponse("Title is required and cannot be empty.");
                }

                bool isDuplicate = await _repository.AnyAsync(t => t.Title.ToLower() == dto.Title.ToLower());
                if (isDuplicate)
                {
                    _logger.LogWarning("Duplicate Todo item title: {Title}. Creation failed.", dto.Title);
                    return BaseResponse<TodoItem>.FailureResponse("A Todo item with this title already exists.", 400);
                }

                var todoItem = _mapper.Map<TodoItem>(dto);
                todoItem.CreatedDate = DateTime.UtcNow;

                _logger.LogInformation("Creating Todo item with title {Title}", dto.Title);

                await _repository.AddAsync(todoItem);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Successfully created Todo item with ID {Id}", todoItem.Id);

                return BaseResponse<TodoItem>.SuccessResponse(todoItem, "Todo item created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Todo item.");
                return BaseResponse<TodoItem>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        public async Task<BaseResponse<TodoItem>> UpdateAsync(int id, UpdateTodoDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    _logger.LogWarning("Attempt to update Todo item with empty title.");
                    return BaseResponse<TodoItem>.FailureResponse("Title is required and cannot be empty.");
                }

                var existingTodo = await _repository.GetByIdAsync(id);
                if (existingTodo == null)
                {
                    _logger.LogWarning("Todo item with ID {Id} not found for update.", id);
                    return BaseResponse<TodoItem>.FailureResponse($"Todo item with ID {id} not found.", 404);
                }

                bool isDuplicate = await _repository.AnyAsync(t => t.Title.ToLower() == dto.Title.ToLower() && t.Id != id);
                if (isDuplicate)
                {
                    _logger.LogWarning("Duplicate Todo item title: {Title} for update. Update failed.", dto.Title);
                    return BaseResponse<TodoItem>.FailureResponse("A Todo item with this title already exists.", 400);
                }

                _mapper.Map(dto, existingTodo);
                _repository.Update(existingTodo);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Successfully updated Todo item with ID {Id}.", id);

                return BaseResponse<TodoItem>.SuccessResponse(existingTodo, "Todo item updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Todo item.");
                return BaseResponse<TodoItem>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        public async Task<BaseResponse<object>> CompleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Marking Todo item with ID {Id} as completed.", id);

                var success = await _repository.MarkAsCompletedAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Todo item with ID {Id} not found for completion.", id);
                    return BaseResponse<object>.FailureResponse($"Todo item with ID {id} not found.", 404);
                }

                await _repository.SaveChangesAsync();
                _logger.LogInformation("Todo item with ID {Id} marked as completed.", id);

                return BaseResponse<object>.SuccessResponse(success, "Todo item marked as completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while marking Todo item as completed.");
                return BaseResponse<object>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        public async Task<BaseResponse<object>> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Todo item with ID {Id}.", id);

                var todoItem = await _repository.GetByIdAsync(id);
                if (todoItem == null)
                {
                    _logger.LogWarning("Todo item with ID {Id} not found for deletion.", id);
                    return BaseResponse<object>.FailureResponse($"Todo item with ID {id} not found.", 404);
                }

                _repository.Delete(todoItem);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted Todo item with ID {Id}.", id);

                return BaseResponse<object>.SuccessResponse(null, "Todo item deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Todo item.");
                return BaseResponse<object>.FailureResponse($"An error occurred: {ex.Message}");
            }
        }
    }
}

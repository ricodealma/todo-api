using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.Infra.Data.Todo.Entities
{
    public class TodoDao(ILogger<TodoDao> logger, ITodoContext todoContext) : ITodoDao
    {
        private readonly ILogger<TodoDao> _logger = logger;
        private readonly ITodoContext _todoContext = todoContext;
        public async Task<Tuple<TodoDto?, ErrorResult>> SelectByIdAsync(Guid id)
        {
            try
            {
                var todoDto = await _todoContext.Todo
                    .Include(todo => todo.Status)
                    .Include(todo => todo.Items)
                    .Include(todo => todo.Client)
                    .FirstOrDefaultAsync(todo => todo.Id == id);

                if (todoDto == null)
                {
                    return new(null, new ErrorResult
                    {
                        Error = true,
                        StatusCode = ErrorCode.NotFound,
                        Id = id.ToString(),
                        Message = "Todo not found for the given ID"
                    });
                }

                return new(todoDto, new());
            }
            catch (Exception e)
            {
                string error = JsonConvert.SerializeObject(e);
                _logger.LogError(error);

                return new(null, new ErrorResult
                {
                    Error = true,
                    Message = error,
                    StatusCode = ErrorCode.InternalServerError,
                    Id = id.ToString()
                });
            }
        }

        public async Task<Tuple<TodoDto?, ErrorResult>> InsertAsync(TodoDto todo)
        {
            await using var transaction = await _todoContext.Database.BeginTransactionAsync();
            try
            {
                var result = await _todoContext.Todo.AddAsync(todo);
                await _todoContext.SaveChangesAsync();

                if (result.Entity.Id == default)
                {
                    await transaction.RollbackAsync();
                    return new(null, new()
                    {
                        Error = true,
                        StatusCode = ErrorCode.InternalServerError,
                        Message = $"Unexpected Error While inserting todo: {JsonConvert.SerializeObject(todo)}"
                    });
                }

                await transaction.CommitAsync();
                return new(result.Entity, new());

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Unexpected error: {ex.Message} - {JsonConvert.SerializeObject(todo)}");
                return new(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"{JsonConvert.SerializeObject(todo)}"
                });
            }
        }

        public async Task<Tuple<TodoDto?, ErrorResult>> PutTodoAsync(Guid todoId, TodoDto todo)
        {
            try
            {
                var todo = await _todoContext.Todo
                    .Include(todo => todo.Status)
                    .Include(todo => todo.Items)
                    .Include(todo => todo.Client)
                    .FirstOrDefaultAsync(todo => todo.Id == todoId);

                if (todo == null)
                    return Tuple.Create<TodoDto?, ErrorResult>(null, new()
                    {
                        Error = true,
                        Id = todoId.ToString(),
                        Message = "Couldn't find todo for that id",
                        StatusCode = ErrorCode.NotFound
                    });

                todo.StatusId = (int)status;

                await _todoContext.SaveChangesAsync();

                return Tuple.Create<TodoDto?, ErrorResult>(todo, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return Tuple.Create<TodoDto?, ErrorResult>(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to update todo status with error: {JsonConvert.SerializeObject(e)}"
                });
            }
        }
    }
}

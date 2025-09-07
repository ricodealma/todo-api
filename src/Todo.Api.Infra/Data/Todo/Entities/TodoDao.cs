using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging;
using Multipay.Receivable.Microservice.Api.Domain.SeedWork;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.Infra.Data.Todo.Entities
{
    public class TodoDao(ILogger<TodoDao> logger, ITodoContext todoContext) : ITodoDao
    {
        private readonly ILogger<TodoDao> _logger = logger;
        private readonly ITodoContext _todoContext = todoContext;
        public async Task<Tuple<Search<TodoDto>?, ErrorResult>> SelectByFilterAsync(Filter filter)
        {
            try
            {
                Search<TodoDto> search = new();
                int skip = Math.Abs(filter.Paging.PerPage * (filter.Paging.Page - 1));

                List<Expression<Func<TodoDto, bool>>> filters = [];

                if (filter.Id != null)
                    filters.Add(x => x.Id == filter.Id);

                if (!string.IsNullOrEmpty(filter.Title))
                    filters.Add(x => EF.Functions.Like(x.Title, $"%{filter.Title}%"));

                if (filter.IsCompleted != null)
                    filters.Add(x => x.IsCompleted == filter.IsCompleted);


                var whereFilter = DynamicFilter.GenerateFilter(filters) ?? (x => true);

                var totalQuery = _todoContext.Todo.Where(whereFilter).AsNoTracking().Select(x => x.Id);
                search.Paging.CurrentPage = filter.Paging.Page;
                search.Paging.PerPage = filter.Paging.PerPage > default(int) ? filter.Paging.PerPage : 10;
                search.Paging.Total = await totalQuery.CountAsync();
                search.Paging.Pages = Convert.ToInt32(Math.Ceiling((double)search.Paging.Total / filter.Paging.PerPage));
                search.Paging.Pages = search.Paging.Total > default(int) && search.Paging.Pages == default ? 1 : search.Paging.Pages;

                IQueryable<TodoDto?> query;

                var baseQuery = _todoContext.Todo
                                 .Where(whereFilter)
                                 .AsNoTracking();


                query = baseQuery
                    .OrderBy(x => x.IsCompleted)
                    .ThenBy(x => x.Id);


                query = query.Skip(skip).Take(search.Paging.PerPage);
                search.Data = await query.ToListAsync();

                if (search.Data == null)
                    return Tuple.Create<Search<TodoDto>?, ErrorResult>(null, new()
                    {
                        Error = true,
                        StatusCode = ErrorCode.NotFound,
                        Message = "No todos found for the given filter."
                    });

                return Tuple.Create<Search<TodoDto>?, ErrorResult>(search, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return Tuple.Create<Search<TodoDto>?, ErrorResult>(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to retrieve todos with error: {JsonConvert.SerializeObject(e)}"
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

        public async Task<Tuple<TodoDto?, ErrorResult>> PutTodoAsync(Guid id, TodoDto todo)
        {
            try
            {
                var currentTodo = await _todoContext.Todo.FindAsync(id);

                if (currentTodo == null)
                    return new(null, new()
                    {
                        Error = true,
                        Id = id.ToString(),
                        Message = "Couldn't find todo for that id",
                        StatusCode = ErrorCode.NotFound
                    });

                currentTodo.Title = todo.Title;
                currentTodo.IsCompleted = todo.IsCompleted;

                await _todoContext.SaveChangesAsync();

                return new(todo, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return new(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to update todo with error: {JsonConvert.SerializeObject(e)}"
                });
            }
        }

        public async Task<Tuple<TodoDto?, ErrorResult>> DeleteTodoByIdAsync(Guid id)
        {
            try
            {
                var todoQuery = _todoContext.Todo.Where(td => td.Id == id);

                var todo = await todoQuery.FirstOrDefaultAsync();
                var deletedRows = await todoQuery.ExecuteDeleteAsync();

                if (deletedRows == 0)
                    return new(null, new()
                    {
                        Error = true,
                        Id = id.ToString(),
                        Message = "No rows were deleted",
                        StatusCode = ErrorCode.NotFound
                    });

                await _todoContext.SaveChangesAsync();

                return new(todo, new());
            }
            catch (Exception e)
            {
                _logger.LogError(JsonConvert.SerializeObject(e));
                return new(null, new()
                {
                    Error = true,
                    StatusCode = ErrorCode.InternalServerError,
                    Message = $"Failed to delete todo with error: {JsonConvert.SerializeObject(e)}"
                });
            }
        }
    }
}

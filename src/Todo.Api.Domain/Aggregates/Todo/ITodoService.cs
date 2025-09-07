using Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging;
using Todo.Api.Domain.Aggregates.Todo.Entities;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.Domain.Aggregates.Todo
{
    public interface ITodoService
    {
        Task<Tuple<TodoModel?, ErrorResult>> InsertTodoAsync(TodoCreateRequest todo);
        Task<Tuple<TodoModel?, ErrorResult>> UpdateTodoByIdAsync(Guid id, TodoModel todo);
        Task<Tuple<Search<TodoModel>?, ErrorResult>> SelectTodoByFilterAsync(Filter filter);
        Task<Tuple<TodoModel?, ErrorResult>> DeleteTodoByIdAsync(Guid id);
    }
}

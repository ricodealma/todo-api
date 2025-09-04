using Todo.Api.Domain.Aggregates.Todo.Entities;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.Domain.Aggregates.Todo
{
    public interface ITodoRepository
    {
        Task<Tuple<TodoModel?, ErrorResult>> DeleteTodoByIdAsync(Guid id);
        Task<Tuple<TodoModel?, ErrorResult>> InsertTodoAsync(TodoCreateRequest todo);
        Task<Tuple<TodoModel?, ErrorResult>> SelectTodoByFilterAsync(Filter filter);
        Task<Tuple<TodoModel?, ErrorResult>> UpdateTodoAsync(Guid id, TodoModel request);
    }
}

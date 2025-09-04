using Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging;
using Todo.Api.Domain.Aggregates.Todo.Entities;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.Domain.Aggregates.Todo
{
    public interface ITodoRepository
    {
        Task<Tuple<TodoModel?, ErrorResult>> DeleteTodoByIdAsync(Guid id);
        Task<Tuple<TodoModel?, ErrorResult>> InsertTodoAsync(TodoModel todo);
        Task<Tuple<Search<TodoModel>?, ErrorResult>> SelectTodoByFilterAsync(Filter filter);
        Task<Tuple<TodoModel?, ErrorResult>> UpdateTodoAsync(Guid id, TodoModel request);
    }
}

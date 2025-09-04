using Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.Infra.Data.Todo.Entities
{
    public interface ITodoDao
    {
        Task<Tuple<TodoDto?, ErrorResult>> InsertAsync(TodoDto todoDto);
        Task<Tuple<Search<TodoDto>?, ErrorResult>> SelectByFilterAsync(Filter filter);
        Task<Tuple<TodoDto?, ErrorResult>> PutTodoAsync(Guid todoId, TodoDto todo);
        Task<Tuple<TodoDto?, ErrorResult>> DeleteTodoByIdAsync(Guid id);
    }

}

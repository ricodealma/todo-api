using Todo.Api.Domain.SeedWork.ErrorResult;

namespace Todo.Api.Infra.Data.Todo.Entities
{
    public interface ITodoDao
    {
        Task<Tuple<TodoDto?, ErrorResult>> InsertAsync(TodoDto todoDto);
        Task<Tuple<TodoDto?, ErrorResult>> SelectByIdAsync(Guid id);
        Task<Tuple<TodoDto?, ErrorResult>> PutTodoAsync(Guid todoId, TodoDto todo);

    }

}

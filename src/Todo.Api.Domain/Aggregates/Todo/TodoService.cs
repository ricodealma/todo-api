using Todo.Api.Domain.SeedWork.ErrorResult;
using Todo.Api.Domain.SeedWork;
using Todo.Api.Domain.Aggregates.Todo.Entities;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;

namespace Todo.Api.Domain.Aggregates.Todo
{
    public sealed class TodoService(ITodoRepository todoRepository, EnvironmentKey environmentKey) : ITodoService
    {
        private readonly ITodoRepository _todoRepository = todoRepository;
        private readonly EnvironmentKey _environmentKey = environmentKey;


        public async Task<Tuple<TodoModel?, ErrorResult>> InsertTodoAsync(TodoCreateRequest request) => await _todoRepository.InsertTodoAsync(request);
        public async Task<Tuple<TodoModel?, ErrorResult>> UpdateTodoByIdAsync(Guid id) => await _todoRepository.UpdateTodoAsync(id, new TodoModel());
        public async Task<Tuple<TodoModel?, ErrorResult>> SelectTodoByFilterAsync(Filter filter) => await _todoRepository.SelectTodoByFilterAsync(filter);
        public async Task<Tuple<TodoModel?, ErrorResult>> DeleteTodoByIdAsync(Guid id) => await _todoRepository.DeleteTodoByIdAsync(id);
    }
}

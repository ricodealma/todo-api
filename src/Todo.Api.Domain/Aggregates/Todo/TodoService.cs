using Todo.Api.Domain.SeedWork.ErrorResult;
using Todo.Api.Domain.Aggregates.Todo.Entities;
using Todo.Api.Domain.Aggregates.Todo.Entities.Filter;
using Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging;

namespace Todo.Api.Domain.Aggregates.Todo
{
    public sealed class TodoService(ITodoRepository todoRepository) : ITodoService
    {
        private readonly ITodoRepository _todoRepository = todoRepository;
        public async Task<Tuple<TodoModel?, ErrorResult>> InsertTodoAsync(TodoCreateRequest request) => await _todoRepository.InsertTodoAsync(request.ToModel());
        public async Task<Tuple<TodoModel?, ErrorResult>> UpdateTodoByIdAsync(Guid id, TodoModel todo) => await _todoRepository.UpdateTodoAsync(id, todo);
        public async Task<Tuple<Search<TodoModel>?, ErrorResult>> SelectTodoByFilterAsync(Filter filter) => await _todoRepository.SelectTodoByFilterAsync(filter);
        public async Task<Tuple<TodoModel?, ErrorResult>> DeleteTodoByIdAsync(Guid id) => await _todoRepository.DeleteTodoByIdAsync(id);
    }
}

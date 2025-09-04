using Todo.Api.Domain.Aggregates.Todo.Entities;
using Todo.Api.Infra.Data.Todo.Entities;

namespace Todo.Api.Infra.Data.Todo.Entities
{
    public static class TodoExtensions
    {
        public static TodoDto FromDomain(this TodoCreateRequest todo)
        {
            return new()
            {
                IsCompleted = todo.IsCompleted,
                Title = todo.Title,
            };
        }

        public static TodoModel ToDomain(this TodoDto dto)
        {
            return new TodoModel
            {
                Id = dto.Id,
                StatusId = dto.StatusId,
                ClientId = dto.ClientId,
                Status = dto.Status?.ToDomain(),
                Items = dto.Items?.ToDomain() ?? [],
                Client = dto.Client?.ToDomain() ?? new(),
            };
        }

        public static List<TodoModel> ToDomain(this List<TodoDto> todoDtos) => todoDtos.Select(ToDomain).ToList();
    }
}

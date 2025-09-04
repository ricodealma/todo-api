using Todo.Api.Domain.Aggregates.Todo.Entities;

namespace Todo.Api.Infra.Data.Todo.Entities
{
    public static class TodoExtensions
    {
        public static TodoDto ToDto(this TodoModel todo)
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
                IsCompleted = dto.IsCompleted,
                Title = dto.Title,
            };
        }

        public static List<TodoModel> ToDomain(this List<TodoDto> todoDtos) => todoDtos.Select(ToDomain).ToList();
    }
}

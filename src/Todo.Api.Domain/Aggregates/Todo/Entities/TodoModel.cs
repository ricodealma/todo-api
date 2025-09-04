namespace Todo.Api.Domain.Aggregates.Todo.Entities;

public sealed class TodoModel
{
    public Guid Id { get; set; }
    public bool IsCompleted { get; set; }
    public string Title { get; set; } = string.Empty;
}

public static class TodoModelExtensions
{
    public static TodoModel ToModel(this TodoCreateRequest request)
    {
        return new TodoModel
        {
            Id = Guid.CreateVersion7(),
            IsCompleted = request.IsCompleted,
            Title = request.Title,
        };
    }
}

namespace Todo.Api.Domain.Aggregates.Todo.Entities;

public sealed class TodoModel
{
    public Guid Id { get; set; }
    public bool IsCompleted { get; set; }
    public string Title { get; set; } = string.Empty;
}

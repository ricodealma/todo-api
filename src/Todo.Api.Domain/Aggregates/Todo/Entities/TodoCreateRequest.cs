namespace Todo.Api.Domain.Aggregates.Todo.Entities;

public sealed class TodoCreateRequest
{
    public bool IsCompleted { get; set; }
    public string Title { get; set; } = string.Empty;
}

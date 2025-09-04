namespace Todo.Api.Domain.Aggregates.Todo.Entities.Filter
{
    public sealed class Filter
    {
        public FilterPaging Paging { get; set; } = new();
        public Guid? Id { get; set; }
        public bool? IsCompleted { get; set; }
        public string? Title { get; set; }

    }
}

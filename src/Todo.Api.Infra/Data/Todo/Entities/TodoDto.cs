using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Api.Infra.Data.Todo.Entities
{
    [Table("Todo")]
    public record TodoDto
    {
        public Guid Id { get; set; }
        public bool IsCompleted { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}

using System.Text.Json.Serialization;

namespace Todo.Api.Domain.SeedWork.ErrorResult
{
    public sealed class ErrorResult : IErrorResult
    {
        [JsonIgnore]
        public bool Error { get; set; }
        public string Id { get; set; } = string.Empty;
        public string Type { get => StatusCode.ToString(); }
        public string Message { get; set; } = string.Empty;
        public ErrorCode StatusCode { get; set; }
    }
}

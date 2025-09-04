using Multipay.Receivable.Microservice.Api.Domain.Aggregates.Multipay.Entities.Filter;

namespace Todo.Api.Domain.Aggregates.Todo.Entities.Filter
{
    public sealed class FilterPaging
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public Sort Sort { get; set; } = Sort.Id;
        public SortCriteria SortCriteria { get; set; } = SortCriteria.Ascending;
    }
}

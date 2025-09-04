namespace Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging
{
    public sealed class Search<T>() : ISearch<T>
    {
        public IPaging Paging { get; set; } = new Paging();
        public List<T> Data { get; set; } = [];
    }
}

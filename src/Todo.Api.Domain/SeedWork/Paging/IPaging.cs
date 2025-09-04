namespace Multipay.Receivable.Microservice.Api.Domain.SeedWork.Paging
{
    public interface IPaging
    {
        int Total { get; set; }
        int CurrentPage { get; set; }
        int PerPage { get; set; }
        int Pages { get; set; }
    }
}

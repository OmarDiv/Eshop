namespace Shared.Pagination
{
    public record PaginationRequest(int PageNumber = 1, int PageSize = 10);
}

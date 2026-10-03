namespace Catalog.Products.Feature.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult>;
public record GetProductsResult(PaginatedList<ProductDto> ProductDtos);

public class GetProductsHandler(CatalogDbContext _context) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
        .AsNoTracking()
        .ProjectToType<ProductDto>();


        var paginatedList = await PaginatedList<ProductDto>.CreateAsync(query, request.PaginationRequest.PageNumber, request.PaginationRequest.PageSize, cancellationToken);

        return new GetProductsResult(paginatedList);
    }
}


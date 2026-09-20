namespace Catalog.Products.Feature.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<ProductDto> ProductDtos);
public class GetProductsHandler(CatalogDbContext _context) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return new GetProductsResult
            (await _context
            .Products
            .AsNoTracking()
            .ProjectToType<ProductDto>()
            .ToListAsync(cancellationToken));
    }
}


using Catalog.Products.Dtos;

namespace Catalog.Products.Feature.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);
public class GetProductsByCategoryHandler(CatalogDbContext _context) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        return new GetProductByCategoryResult(await _context
            .Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(request.Category))
            .OrderBy(p => p.Name)
            .ProjectToType<ProductDto>()
            .ToListAsync(cancellationToken));
    }
}

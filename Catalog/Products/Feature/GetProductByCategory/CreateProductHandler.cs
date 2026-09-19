using Catalog.Products.Dtos;

namespace Catalog.Products.Feature.GetProductByCategory;

public record GetProductByCategoryCommand(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);
public class GetProductByCategoryHandler(CatalogDbContext _context) : IQueryHandler<GetProductByCategoryCommand, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryCommand request, CancellationToken cancellationToken)
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

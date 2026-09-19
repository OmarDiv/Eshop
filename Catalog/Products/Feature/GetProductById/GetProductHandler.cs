using Catalog.Products.Dtos;

namespace Catalog.Products.Feature.GetProductById;

public record GetProductQuery(Guid ProductId) : IQuery<GetProductResult>;
public record GetProductResult(ProductDto Product);
public class GetProductHandler(CatalogDbContext _context) : IQueryHandler<GetProductQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .ProjectToType<ProductDto>()
            .SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new Exception($"Product with id {request.ProductId} Not Found");
        }

        return new GetProductResult(product!);

    }

}


namespace Catalog.Products.Feature.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(ProductDto Product);
public class GetProductByIdHandler(CatalogDbContext _context) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .ProjectToType<ProductDto>()
            .SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new Exception($"Product with id {request.ProductId} Not Found");
        }

        return new GetProductByIdResult(product!);

    }

}


namespace Catalog.Products.Feature.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(ProductDto Product);
public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}

public class GetProductByIdHandler(CatalogDbContext _context) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == request.ProductId)
            .ProjectToType<ProductDto>()
            .SingleOrDefaultAsync(cancellationToken);



        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        return new GetProductByIdResult(product);

    }

}


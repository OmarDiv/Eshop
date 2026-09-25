
namespace Catalog.Products.Feature.GetProductByCategory;

public record GetProductByCategoryQuery(PaginationRequest Pagination, string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(PaginatedList<ProductDto> Products);
public class GetProductByCategoryQueryValidator : AbstractValidator<GetProductByCategoryQuery>
{
    public GetProductByCategoryQueryValidator()
    {
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
    }
}
public class GetProductsByCategoryHandler(CatalogDbContext _context) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context
            .Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(request.Category))
            .OrderBy(p => p.Name)
            .ProjectToType<ProductDto>();
        var data = await PaginatedList<ProductDto>.CreateAsync(query, request.Pagination.PageNumber, request.Pagination.PageSize);
        return new GetProductByCategoryResult(data);
    }
}

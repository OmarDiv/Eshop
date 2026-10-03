using Shared.Contracts.CQRS;
using Shared.Contracts.Dtos;

namespace Catalog.Contracts.Products.Feature.GetProductByIdQuery
{
    public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(ProductDto Product);
}

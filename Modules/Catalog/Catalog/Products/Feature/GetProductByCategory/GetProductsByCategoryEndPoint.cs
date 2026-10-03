
namespace Catalog.Products.Feature.GetProductByCategory
{
    public record GetProductByCategoryResponse(PaginatedList<ProductDto> Products);

    public class GetProductsByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async ([AsParameters] PaginationRequest paginationRequest, string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(paginationRequest, category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Get products by category");
        }
    }
}

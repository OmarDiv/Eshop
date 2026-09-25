namespace Catalog.Products.Feature.GetProducts
{
    public record GetProductsResponse(PaginatedList<ProductDto> ProductDtos);
    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery(paginationRequest));
                var response = new GetProductsResponse(result.ProductDtos);
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all products")
            .WithDescription("Get all products");
        }
    }
}

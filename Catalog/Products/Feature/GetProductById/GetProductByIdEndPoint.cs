namespace Catalog.Products.Feature.GetProductById
{
    public record GetProductByIdResponse(ProductDto Product);
    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapGet("/products/{productId}", async (Guid productId, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(productId));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get a product by its ID.")
            .WithDescription("Get a product by its ID.");
        }
    }
}

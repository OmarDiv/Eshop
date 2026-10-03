public record DeleteProductResponse(bool IsSuccess);

namespace Catalog.Products.Feature.DeleteProduct
{
    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{productId}", async (Guid productId, ISender sender) =>
            {
                var command = new DeleteProductCommand(productId);
                var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("DeleteProduct")
            .WithDescription("Delete a product by its ID")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete a product by its ID");
        }
    }
}

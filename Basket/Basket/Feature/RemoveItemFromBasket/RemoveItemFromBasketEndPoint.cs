namespace Basket.Basket.Feature.RemoveItemFromBasket
{
    public record RemoveItemFromBasketResponse(Guid Id);
    public class RemoveItemFromBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}/item/{productId}", async (string userName, Guid productId, ISender sender) =>
            {
                var command = new RemoveItemFromBasketCommand(userName, productId);
                var result = await sender.Send(command);
                var response = result.Adapt<RemoveItemFromBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("RemoveItemFromBasket")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Removes an item from the authenticated user's basket.");
        }
    }
}

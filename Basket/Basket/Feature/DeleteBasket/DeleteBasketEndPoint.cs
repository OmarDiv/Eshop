namespace Basket.Basket.Feature.DeleteBasket
{
    public record DeleteBasketResponse(bool Success);
    public class DeleteBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{UserName}", async (string UserName, ISender sender) =>
            {
                var command = new DeleteBasketCommand(UserName);
                var result = await sender.Send(command);
                return Results.Ok(new DeleteBasketResponse(result.Success));
            })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Deletes a basket by User Name.");
        }
    }
}

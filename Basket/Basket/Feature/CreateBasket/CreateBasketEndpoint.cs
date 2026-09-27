namespace Basket.Basket.Feature.CreateBasket
{
    public record CreateBasketRequest(ShoppingCartDto ShoppingCart);
    public record CreateBasketResponse(Guid Id);
    public class CreateBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (CreateBasketRequest request,ISender sender) =>
            {
                var command = new CreateBasketCommand(request.ShoppingCart);
                var result = await sender.Send(command);
                return Results.Ok(new CreateBasketResponse(result.Id));
            })
            .WithName("CreateBasket")
            .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Creates a new basket.");
        }
    }
}

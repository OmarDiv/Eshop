namespace Basket.Basket.Feature.AddItemToBasket
{
    public record AddItemToBasketRequest(ShoppingCartItemDto ShoppingCartItem);
    public record AddItemToBasketResponse(Guid Id);
    public class AddItemIntoBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/{userName}/items", async ([FromRoute] string userName, [FromBody] AddItemToBasketRequest request, ISender sender) =>
            {
                var command = new AddItemToBasketCommand(userName, request.ShoppingCartItem);
                var result = await sender.Send(command);
                var response = result.Adapt<AddItemToBasketResponse>();
                return Results.Created($"/basket/{response.Id}", response);
            })
            .WithName("AddItemToBasket")
            .Produces<AddItemToBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Adds an item to a basket.")
            .WithDescription("Adds an item to a basket.");
        }
    }
}

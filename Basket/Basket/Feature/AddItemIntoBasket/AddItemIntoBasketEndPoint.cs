using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Feature.AddItemToBasket
{
    public record AddItemToBasketRequest(string UserName, ShoppingCartItemDto Item);
    public record AddItemToBasketResponse(bool Success);
    public class AddItemIntoBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/{userName}/item", async ([FromRoute]string userName, [FromBody]AddItemToBasketRequest request, ISender sender) =>
            {
                var command = new AddItemToBasketCommand(userName, request.Item);
                var result = await sender.Send(command);
                return Results.Ok(new AddItemToBasketResponse(result.Success));
            })
            .WithName("AddItemToBasket")
            .Produces<AddItemToBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Adds an item to a basket.");
        }
    }
}

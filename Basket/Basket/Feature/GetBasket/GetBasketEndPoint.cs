
namespace Basket.Basket.Feature.GetBasket
{
    public record GetBaskeResponse(ShoppingCartDto ShoppingCart);
    public class GetBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var query = new GetBasketQuery(userName);
                var result = await sender.Send(query);
                var response = result.Adapt<GetBaskeResponse>();
                return Results.Ok(response);
            })
             .WithName("GetBasket")
             .Produces<GetBaskeResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithDescription("Get basket by user name");
        }
    }
}

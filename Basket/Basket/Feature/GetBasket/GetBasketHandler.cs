
namespace Basket.Basket.Feature.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCartDto ShoppingCart);
    public class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
    {
        public GetBasketQueryValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.");
        }
    }
    public class GetBasketHandler(BasketDbContext _context) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        { 
            var Basket = await _context
                .ShoppingCarts
                .AsNoTracking()
                .Where(c => c.UserName == request.UserName)
                .Include(x => x.ShoppingCartItems)
                .ProjectToType<ShoppingCartDto>()
                .SingleOrDefaultAsync(cancellationToken);
            if(Basket is null)
            {
                throw new BasketNotFoundException(request.UserName);
            }
            return new GetBasketResult(Basket);
        } 
    }
}

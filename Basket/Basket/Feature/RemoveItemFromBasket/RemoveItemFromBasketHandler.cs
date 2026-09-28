namespace Basket.Basket.Feature.RemoveItemFromBasket
{
    public record RemoveItemFromBasketCommand(string UserName, Guid ProductId) : ICommand<RemoveItemFromBasketResult>;
    public record RemoveItemFromBasketResult(Guid Id);

    public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
    {
        public RemoveItemFromBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
        }
    }

    public class RemoveItemFromBasketHandler(BasketDbContext _context)
        : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
    {
        public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(x => x.ShoppingCartItems)
                .SingleOrDefaultAsync(c => c.UserName == request.UserName, cancellationToken);

            if (shoppingCart is null)
                throw new BasketNotFoundException(request.UserName);

            shoppingCart.RemoveItem(request.ProductId);
            await _context.SaveChangesAsync(cancellationToken);

            return new RemoveItemFromBasketResult(shoppingCart.Id);
        }
    }
}
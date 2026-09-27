namespace Basket.Basket.Feature.AddItemToBasket
{
    public record AddItemToBasketCommand(string UserName, ShoppingCartItemDto Item) : ICommand<AddItemToBasketResult>;

    public record AddItemToBasketResult(bool Success);

    public class AddItemToBasketCommandValidator : AbstractValidator<AddItemToBasketCommand>
    {
        public AddItemToBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.");
            RuleFor(x => x.Item).NotNull().WithMessage("Item is required.").SetValidator(new ShoppingCartItemDtoValidator());
        }
    }

    public class AddItemIntoBasketHandler(BasketDbContext _context) : ICommandHandler<AddItemToBasketCommand, AddItemToBasketResult>
    {
        public async Task<AddItemToBasketResult> Handle(AddItemToBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(c => c.UserName == request.UserName);
            if (shoppingCart is null)
            {
                throw new BasketNotFoundException(request.UserName);
            }
            shoppingCart.AddItem(
                request.Item.ProductId,
                request.Item.Quantity,
                request.Item.Color,
                request.Item.Price,
                request.Item.ProductName
            );

            await _context.SaveChangesAsync(cancellationToken);
            return new AddItemToBasketResult(true);
        }
    }
}

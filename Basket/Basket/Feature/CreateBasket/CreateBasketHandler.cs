namespace Basket.Basket.Feature.CreateBasket
{
    public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;
    public record CreateBasketResult(Guid Id);
    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("User name is required.");
            RuleForEach(x => x.ShoppingCart.ShoppingCartItems).SetValidator(new ShoppingCartItemDtoValidator());
        }
    }
    public class ShoppingCartItemDtoValidator : AbstractValidator<ShoppingCartItemDto>
    {
        public ShoppingCartItemDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be a positive number.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be a positive number.");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required.");
        }
    }
    public class CreateBasketHandler(BasketDbContext _context) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
    {
        public async Task<CreateBasketResult> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = CreateBasket(request.ShoppingCart);
            await _context.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new CreateBasketResult(shoppingCart.Id);
        }
        private static ShoppingCart CreateBasket(ShoppingCartDto shoppingCartDto)
        {
            var shoppingCart = ShoppingCart.Create(Guid.NewGuid(), shoppingCartDto.UserName);
            shoppingCartDto.ShoppingCartItems.ForEach(item =>
            {
                shoppingCart.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);
            });
            return shoppingCart;
        }
    }

}

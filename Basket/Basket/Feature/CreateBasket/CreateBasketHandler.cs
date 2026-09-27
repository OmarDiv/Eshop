

namespace Basket.Basket.Feature.CreateBasket
{
    public record CreateBasketCommand(ShoppingCartDto ShoppingCartDto) : ICommand<CreateBasketResult>;
    public record CreateBasketResult(Guid Id);
    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCartDto.UserName).NotEmpty();
            RuleForEach(x => x.ShoppingCartDto.Items).SetValidator(new ShoppingCartItemDtoValidator());
        }
    }
    public class ShoppingCartItemDtoValidator : AbstractValidator<ShoppingCartItemDto>
    {
        public ShoppingCartItemDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.ProductName).NotEmpty();
        }
    }
    public class CreateBasketHandler(BasketDbContext _context) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
    {
        public async Task<CreateBasketResult> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = CreateBasket(request.ShoppingCartDto);
            await _context.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new CreateBasketResult(shoppingCart.Id);
        }
        private static ShoppingCart CreateBasket(ShoppingCartDto shoppingCartDto)
        {
            var shoppingCart = ShoppingCart.Create(Guid.NewGuid(), shoppingCartDto.UserName);
            shoppingCartDto.Items.ForEach(item=>
            { 
                shoppingCart.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);
            }); ;
            return shoppingCart;
        }
    }
    
}

namespace Basket.Basket.Feature.AddItemToBasket
{
    public record AddItemToBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem) : ICommand<AddItemToBasketResult>;

    public record AddItemToBasketResult(Guid Id);

    public class AddItemToBasketCommandValidator : AbstractValidator<AddItemToBasketCommand>
    {
        public AddItemToBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.");
            RuleFor(x => x.ShoppingCartItem).NotNull().WithMessage("Item is required.").SetValidator(new ShoppingCartItemDtoValidator());
        }
    }

    public class AddItemIntoBasketHandler(IBasketRepository _basketRepository) : ICommandHandler<AddItemToBasketCommand, AddItemToBasketResult>
    {
        public async Task<AddItemToBasketResult> Handle(AddItemToBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasketAsync(request.UserName , false, cancellationToken);
            shoppingCart.AddItem( 
                request.ShoppingCartItem.ProductId,
                request.ShoppingCartItem.Quantity,
                request.ShoppingCartItem.Color,
                request.ShoppingCartItem.Price,
                request.ShoppingCartItem.ProductName
            );

            await _basketRepository.SaveChangesAsync(cancellationToken);
            return new AddItemToBasketResult(shoppingCart.Id);
        }
    }
}

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

    public class RemoveItemFromBasketHandler(IBasketRepository _basketRepository)
        : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
    {
        public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasketAsync(request.UserName,false, cancellationToken);

            shoppingCart.RemoveItem(request.ProductId);
            await _basketRepository.SaveChangesAsync(cancellationToken);

            return new RemoveItemFromBasketResult(shoppingCart.Id);
        }
    }
}
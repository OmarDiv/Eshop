namespace Basket.Basket.Feature.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool Success);
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.");
        }
    }
    public class DeleteBasketHandler(BasketDbContext _context) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(c => c.UserName == request.UserName, cancellationToken);
            if (shoppingCart is null)
            {
                throw new BasketNotFoundException(request.UserName);
            }

            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync(cancellationToken);
            return new DeleteBasketResult(true);
        }
    }
}

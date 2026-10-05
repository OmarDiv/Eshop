namespace Basket.Basket.Feature.UpdateItemPriceBasket
{
    public record UpdateItemPriceBasketCommand(Guid ProductId, decimal NewPrice) : ICommand<UpdateItemPriceBasketResponse>;
    public record UpdateItemPriceBasketResponse(bool IsSuccess); 

    public class UpdateItemPriceBasketHandler(BasketDbContext _context) : ICommandHandler<UpdateItemPriceBasketCommand, UpdateItemPriceBasketResponse>
    {
        public async Task<UpdateItemPriceBasketResponse> Handle(UpdateItemPriceBasketCommand request, CancellationToken cancellationToken)
        {
            var basketItemToUpdate = await _context.ShoppingCartItems
                .Where(bi => bi.ProductId == request.ProductId)
                .ToListAsync(cancellationToken);
            if (!basketItemToUpdate.Any())
            {
                return new UpdateItemPriceBasketResponse(false);
            }
            foreach (var item in basketItemToUpdate)
            {
                item.UpdatePrice(request.NewPrice);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateItemPriceBasketResponse(true);
        }
    }
}
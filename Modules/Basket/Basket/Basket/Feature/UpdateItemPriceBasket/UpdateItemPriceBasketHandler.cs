using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Basket.Feature.UpdateItemPriceBasket
{
    public record UpdateItemPriceBasketCommand(Guid ProductId, decimal NewPrice) : ICommand<UpdateItemPriceBasketResponse>;
    public record UpdateItemPriceBasketResponse(bool IsSuccess); 

    public class UpdateItemPriceBasketHandler(
        BasketDbContext _context,
        IDistributedCache _cache) : ICommandHandler<UpdateItemPriceBasketCommand, UpdateItemPriceBasketResponse>
    {
        private const string CacheKeyPrefix = "basket";

        public async Task<UpdateItemPriceBasketResponse> Handle(UpdateItemPriceBasketCommand request, CancellationToken cancellationToken)
        {
            var Transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            // Get affected items along with the UserName of their parent basket
            var affectedItems = await _context.ShoppingCartItems
                .Where(bi => bi.ProductId == request.ProductId)
                .Join(
                    _context.ShoppingCarts,
                    item => item.ShoppingCartId,
                    cart => cart.Id,
                    (item, cart) => new { Item = item, UserName = cart.UserName })
                .ToListAsync(cancellationToken);

            if (!affectedItems.Any())
            {
                return new UpdateItemPriceBasketResponse(false);
            }

            foreach (var entry in affectedItems)
            {
                entry.Item.UpdatePrice(request.NewPrice);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Invalidate cache for each affected user's basket
            var affectedUserNames = affectedItems
                .Select(e => e.UserName)
                .Distinct();

            foreach (var userName in affectedUserNames)
            {
                await _cache.RemoveAsync($"{CacheKeyPrefix}_{userName}", cancellationToken);
            }

            await Transaction.CommitAsync(cancellationToken);

            return new UpdateItemPriceBasketResponse(true);
        }
    }
}
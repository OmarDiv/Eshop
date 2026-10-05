namespace Basket.Data.Repository
{
    public class BasketRepository(BasketDbContext _context) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            var query = _context.ShoppingCarts
                .Include(b => b.ShoppingCartItems)
                .Where(b => b.UserName == userName);

            if (asNoTracking)
            {
                query.AsNoTracking();
            }
            var basket = await query.SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return basket ?? throw new BasketNotFoundException(userName);
        }

        public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await _context.ShoppingCarts.AddAsync(basket, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await GetBasketAsync(userName, false, cancellationToken: cancellationToken);

            _context.ShoppingCarts.Remove(basket);
            return await _context.SaveChangesAsync(cancellationToken) > 1;

        }
        public async Task<bool> UpdateItemPriceAsync(Guid productId, decimal newPrice, CancellationToken cancellationToken = default)
        {
            var basketItem = await _context.ShoppingCarts
                .Where(item => item.ShoppingCartItems.Any(cartItem => cartItem.ProductId == productId))
                .FirstOrDefaultAsync(cancellationToken);

            await SaveChangesAsync(basketItem.UserName, cancellationToken);
            var affectedRows = await _context.ShoppingCartItems
                .Where(item => item.ProductId == productId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(item => item.Price, newPrice),
                    cancellationToken);


            return affectedRows > 0;

        }
        public async Task<int> SaveChangesAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

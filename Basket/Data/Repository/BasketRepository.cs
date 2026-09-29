namespace Basket.Data.Repository
{
    public class BasketRepository(BasketDbContext _context) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            var query = _context.ShoppingCarts
                .Include(b => b.ShoppingCartItems)
                .Where(b => b.UserName == userName);

              if(asNoTracking)
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
            var basket = await GetBasketAsync(userName,false ,cancellationToken: cancellationToken);

            _context.ShoppingCarts.Remove(basket);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

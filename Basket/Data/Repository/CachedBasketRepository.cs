using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository(IBasketRepository _basketRepository , IDistributedCache _distributedCache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
          return await _basketRepository.GetBasketAsync(userName, asNoTracking, cancellationToken);
        }

        public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {

            return await _basketRepository.CreateBasketAsync(basket, cancellationToken);

        }

        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _basketRepository.DeleteBasketAsync(userName, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _basketRepository.SaveChangesAsync(cancellationToken);
        }
    }
}

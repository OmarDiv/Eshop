using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository(IBasketRepository _basketRepository, IDistributedCache _distributedCache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            var CachedBasket = await _distributedCache.GetStringAsync($"basket_{userName}", cancellationToken);
            if (CachedBasket != null)
            {
                return JsonSerializer.Deserialize<ShoppingCart>(CachedBasket);
            }
            var basket = await _basketRepository.GetBasketAsync(userName, asNoTracking, cancellationToken);
            if (basket != null)
            {
                await _distributedCache.SetStringAsync($"basket_{userName}", JsonSerializer.Serialize(basket), cancellationToken);
            }
            return basket!;
        }

        public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"basket_{basket.UserName}";
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);
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

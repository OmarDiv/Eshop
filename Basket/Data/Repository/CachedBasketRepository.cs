using Basket.Data.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository(
        IBasketRepository _basketRepository,
        IDistributedCache _distributedCache,
        ILogger<CachedBasketRepository> _logger) : IBasketRepository
    {
        private const string CacheKeyPrefix = "basket";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        private static string BuildCacheKey(string userName) => $"{CacheKeyPrefix}_{userName}";

        public async Task<ShoppingCart> GetBasketAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            if (!asNoTracking)
            {
                return await _basketRepository.GetBasketAsync(userName, asNoTracking, cancellationToken);
            }

            var cacheKey = BuildCacheKey(userName);
            var cachedJson = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            if (cachedJson is not null)
            {
                try
                {
                    var dto = JsonSerializer.Deserialize<ShoppingCartCacheDto>(cachedJson);
                    if (dto is not null)
                    {
                        _logger.LogDebug("Basket cache hit for user {UserName}", userName);
                        return ShoppingCartCacheMapper.ToDomain(dto);
                    }
                }
                catch (JsonException ex)
                {
                    // Corrupted cache entry — treat as cache miss and fall back to repository.
                    // This prevents a single bad entry from crashing the entire request.
                    _logger.LogWarning(ex, "Corrupted basket cache entry for user {UserName}, falling back to repository", userName);
                }
            }

            _logger.LogDebug("Basket cache miss for user {UserName}", userName);
            var basket = await _basketRepository.GetBasketAsync(userName, asNoTracking, cancellationToken);
            await SetCacheAsync(cacheKey, basket, cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            var result = await _basketRepository.CreateBasketAsync(basket, cancellationToken);
            await SetCacheAsync(BuildCacheKey(result.UserName), result, cancellationToken);
            return result;
        }

        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var result = await _basketRepository.DeleteBasketAsync(userName, cancellationToken);
            if (result)
            {
                await _distributedCache.RemoveAsync(BuildCacheKey(userName), cancellationToken);
            }
            return result;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _basketRepository.SaveChangesAsync(cancellationToken);
        }

        private async Task SetCacheAsync(string cacheKey, ShoppingCart cart, CancellationToken cancellationToken)
        {
            var dto = ShoppingCartCacheMapper.ToDto(cart);
            var json = JsonSerializer.Serialize(dto);

            await _distributedCache.SetStringAsync(
                cacheKey,
                json,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheDuration },
                cancellationToken);
        }
    }
}

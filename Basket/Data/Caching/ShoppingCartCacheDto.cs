namespace Basket.Data.Caching;

public sealed class ShoppingCartCacheDto
{
    public required Guid Id { get; init; }
    public required string UserName { get; init; }
    public required List<ShoppingCartItemCacheDto> Items { get; init; }
}

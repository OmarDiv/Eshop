namespace Basket.Data.Caching;

public sealed class ShoppingCartItemCacheDto
{
    public required Guid Id { get; init; }
    public required Guid ShoppingCartId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
    public required string Color { get; init; }
    public required decimal Price { get; init; }
    public required string ProductName { get; init; }
}

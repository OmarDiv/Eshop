namespace Basket.Data.Caching;

internal static class ShoppingCartCacheMapper
{
    public static ShoppingCartCacheDto ToDto(ShoppingCart cart) => new()
    {
        Id = cart.Id,
        UserName = cart.UserName,
        Items = cart.ShoppingCartItems.Select(ToItemDto).ToList()
    };

    private static ShoppingCartItemCacheDto ToItemDto(ShoppingCartItem item) => new()
    {
        Id = item.Id,
        ShoppingCartId = item.ShoppingCartId,
        ProductId = item.ProductId,
        Quantity = item.Quantity,
        Color = item.Color,
        Price = item.Price,
        ProductName = item.ProductName
    };

    public static ShoppingCart ToDomain(ShoppingCartCacheDto dto)
    {
        var cart = ShoppingCart.Create(dto.Id, dto.UserName);

        foreach (var itemDto in dto.Items)
        {
            cart.RestoreItem(
                itemDto.Id,
                itemDto.ShoppingCartId,
                itemDto.ProductId,
                itemDto.Quantity,
                itemDto.Color,
                itemDto.Price,
                itemDto.ProductName);
        }

        return cart;
    }
}

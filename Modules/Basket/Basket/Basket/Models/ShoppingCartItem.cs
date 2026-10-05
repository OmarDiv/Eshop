namespace Basket.Basket.Models
{
    public class ShoppingCartItem : Entity<Guid>
    {
        public Guid ShoppingCartId { get; private set; } = default!;
        public Guid ProductId { get; private set; } = default!;
        public int Quantity { get; internal set; } = default!;
        public string Color { get; private set; } = default!;
        //will come from the Catalog Module
        public decimal Price { get; private set; } = default!;
        public string ProductName { get; private set; } = default!;
        internal ShoppingCartItem(Guid id, Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
        : this(shoppingCartId, productId, quantity, color, price, productName)
        {
            Id = id;
        }
        internal ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
        {
            ShoppingCartId = shoppingCartId;
            ProductId = productId;
            Quantity = quantity;
            Color = color;
            Price = price;
            ProductName = productName;
        }
        public void UpdatePrice(decimal newPrice)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);
            Price = newPrice;
        }

        internal void IncreaseQuantity(int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
            Quantity += amount;
        }
    }
}

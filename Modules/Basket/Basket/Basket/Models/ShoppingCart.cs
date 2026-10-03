namespace Basket.Basket.Models
{
    public class ShoppingCart : AggregateRoot<Guid>
    {
        public string UserName { get; private set; } = default!;

        private readonly List<ShoppingCartItem> _shoppingCartItems = [];

        public IReadOnlyCollection<ShoppingCartItem> ShoppingCartItems => _shoppingCartItems.AsReadOnly();
        public decimal TotalPrice => _shoppingCartItems.Sum(x => x.Price * x.Quantity);

        public static ShoppingCart Create(Guid id, string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);
            var shoppingCart = new ShoppingCart
            {
                Id = id,
                UserName = userName
            };
            return shoppingCart;
        }

        public void AddItem(Guid ProductId, int Quantity, string Color, decimal Price, string ProductName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Price);
            var existingItem = _shoppingCartItems.FirstOrDefault(x => x.ProductId == ProductId && x.Color == Color);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(Quantity);
                return;
            }
            else
            {
                var item = new ShoppingCartItem(this.Id, ProductId, Quantity, Color, Price, ProductName);
                _shoppingCartItems.Add(item);
            }
        }

        public void RemoveItem(Guid ProductId)
        {
            var existingItem = _shoppingCartItems.FirstOrDefault(x => x.ProductId == ProductId);
            if (existingItem != null)
            {
                _shoppingCartItems.Remove(existingItem);
            }
        }

        /// <summary>
        /// Used exclusively by the caching infrastructure to rebuild a cart's items
        /// from a cache snapshot, preserving original item identities.
        /// Not part of the domain's business operations.
        /// </summary>
        internal void RestoreItem(Guid id, Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
        {
            _shoppingCartItems.Add(new ShoppingCartItem(id,shoppingCartId, productId, quantity, color, price, productName));
        }
    }
}

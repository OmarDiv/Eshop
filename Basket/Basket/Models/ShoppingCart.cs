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
                existingItem.Quantity += Quantity;
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
    }
}

using Catalog.Products.Events;
namespace Catalog.Products.Models
{
    public class Product :AggregateRoot<Guid>
    {
        public string Name { get; set; } = default!;
        public List<string> Category { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; }= default!;
        public decimal Price { get; set; }
        public static Product Create(Guid id, string name, List<string> category, string description, string imageFile, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            var Product = new Product
            {
                Name = name,
                Category = category,
                Description = description,
                ImageFile = imageFile,
                Price = price,
            };

            Product.AddDomainEvent(new ProductCreatedEvent(Product));
            return Product;

        }
    }
}

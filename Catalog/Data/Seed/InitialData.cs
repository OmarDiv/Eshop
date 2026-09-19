namespace Catalog.Data.Seed
{
    public static class InitialData
    {
        public static IEnumerable<Product> Products => new List<Product>
        {
            Product.Create(Guid.NewGuid(), "Product 1", new List<string> { "Category 1" }, "Description for Product 1", "image1.jpg", 10.99m),
            Product.Create(Guid.NewGuid(), "Product 2", new List<string> { "Category 2" }, "Description for Product 2", "image2.jpg", 20.99m),
            Product.Create(Guid.NewGuid(), "Product 3", new List<string> { "Category 3" }, "Description for Product 3", "image3.jpg", 30.99m),
            Product.Create(Guid.NewGuid(), "Product 4", new List<string> { "Category 4" }, "Description for Product 4", "image4.jpg", 40.99m),
            Product.Create(Guid.NewGuid(), "Product 5", new List<string> { "Category 5" }, "Description for Product 5", "image5.jpg", 50.99m),
        };
    }
}

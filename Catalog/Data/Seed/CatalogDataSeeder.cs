namespace Catalog.Data.Seed
{
    public class CatalogDataSeeder(CatalogDbContext context) : IDataSeeder
    {
        public async Task SeedAllAsync()
        {
            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }

        }
    }
}

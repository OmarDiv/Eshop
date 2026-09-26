namespace Basket.Data.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {

            builder.Property(x => x.Id);
            builder.HasIndex(x => x.UserName)
                .IsUnique();
            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasMany(x => x.ShoppingCartItems)
                .WithOne()
                .HasForeignKey(x => x.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}

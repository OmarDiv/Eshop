namespace Catalog.Products.Feature.UpdateProduct;

public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Product id is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Product name is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Product price must be a positive value");


    }

}
public class UpdateProductHandler(CatalogDbContext _context) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {

        var product = await _context.Products.FindAsync([request.Product.Id], cancellationToken);

        if (product is null)
            throw new Exception($"Product with id {request.Product.Id} Not Found");

        UpdateProductWithNewVales(product, request.Product);
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }
    private static void UpdateProductWithNewVales(Product product, ProductDto request)
    {
        product.Update(
            request.Name,
            request.Category,
            request.Description,
            request.ImageFile,
            request.Price
        );
    }
}

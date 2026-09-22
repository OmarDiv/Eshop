using Catalog.Products.Dtos;

namespace Catalog.Products.Feature.CreateProduct;

public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid ProductId);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Product).NotNull().WithMessage("Product is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Product name is required");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Product category is required");
        RuleFor(x => x.Product.Description).MaximumLength(500).WithMessage("Product description cannot exceed 500 characters");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Product price must be a positive value");
    }
}

public class CreateProductHandler(CatalogDbContext _context) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {

        var product = CreateNewProduct(request.Product);
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateProductResult(product.Id);
    }

    private static Product CreateNewProduct(ProductDto request)
    {
        return Product.Create(
            Guid.NewGuid(),
            request.Name,
            request.Category,
            request.Description,
            request.ImageFile,
            request.Price
        );
    }

}


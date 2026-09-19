using Catalog.Products.Dtos;

namespace Catalog.Products.Feature.CreateProduct;

public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid ProductId);
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



namespace Catalog.Products.Feature.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}
public class DeleteProductHandler(CatalogDbContext _context) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync([request.ProductId], cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}

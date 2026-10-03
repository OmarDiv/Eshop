using Microsoft.Extensions.Logging;

namespace Catalog.Products.EventHandlers
{
    public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) : INotificationHandler<ProductCreatedEvent>
    {
        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Product created event handled. Product ID: {notification.Product.Id}");
            await Task.CompletedTask;
        }
    }
}

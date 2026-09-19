using Microsoft.Extensions.Logging;

namespace Catalog.Products.EventHandlers
{
    public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger) : INotificationHandler<ProductPriceChangedEvent>
    {
        public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Product price changed event handled. Product ID: {notification.Product.Id} And Price: {notification.Product.Price}");
            await Task.CompletedTask;
        }
    }
}

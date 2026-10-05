using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers
{
    public class ProductPriceChangedEventHandler(IBus bus, ILogger<ProductPriceChangedEventHandler> logger) : INotificationHandler<ProductPriceChangedEvent>
    {
        public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Product price changed event handled. Product ID: {notification.Product.Id} And Price: {notification.Product.Price}");
            var productPriceChangedEvent = new ProductPriceChangedIntegrationEvent
            {

                ProductId = notification.Product.Id,
                Name = notification.Product.Name,
                Categories = notification.Product.Category,
                Description = notification.Product.Description,
                ImageUrl = notification.Product.ImageFile,
                Price = notification.Product.Price,
            };
            await bus.Publish(productPriceChangedEvent, cancellationToken);
        }
    }
}

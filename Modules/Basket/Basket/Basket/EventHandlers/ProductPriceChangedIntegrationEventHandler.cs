using Basket.Basket.Feature.UpdateItemPriceBasket;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers
{
    public class ProductPriceChangedIntegrationEventHandler(ISender sender,ILogger<ProductPriceChangedIntegrationEventHandler> logger) : IConsumer<ProductPriceChangedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
        {
            logger.LogInformation($"[START]Integration Event Handled Type: {context.Message.GetType().Name} And Price: {context.Message.Price}");
            var result = await sender.Send(new UpdateItemPriceBasketCommand(context.Message.ProductId, context.Message.Price));
            if (!result.IsSuccess)
                logger.LogError($"Failed to update item price for Product ID: {context.Message.ProductId}");
            logger.LogInformation($"[END]Integration Event Handled Type: {context.Message.GetType().Name} And Price: {context.Message.Price} - Result: {result.IsSuccess}");
        }
    }
}

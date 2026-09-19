using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shared.Data.Interceptors
{
    public class DispatchDomainEventInterceptor(IMediator _mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvent(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvent(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private async Task DispatchDomainEvent(DbContext? context)
        {
            if (context is null)
                return;
            var aggregate = context
                .ChangeTracker
                .Entries<IAggregate>()
                .Select(a => a.Entity);
            var domainEvents = aggregate.SelectMany(a => a.DomainEvents).ToList();
            aggregate.ToList().ForEach(domainEvent => domainEvent.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent);
        }
    }
}

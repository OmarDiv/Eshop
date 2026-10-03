using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace Shared.Data.Interceptors
{
    public class AuditableEntityInterceptor(IHttpContextAccessor _httpContextAccessor) : SaveChangesInterceptor
    {

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntitys(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntitys(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private void UpdateEntitys(DbContext? context)
        {
            if (context == null) return;
            var entries = context.ChangeTracker.Entries<IEntity>();
            var user = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = user;
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = user;
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified || entry.HasChangedOwendEntities())
                {
                    entry.Entity.LastModifiedBy = user;
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                }

            }

        }
    }

}
public static class EntityEntryExtensions
{
    public static bool HasChangedOwendEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified)
        );
    }
}


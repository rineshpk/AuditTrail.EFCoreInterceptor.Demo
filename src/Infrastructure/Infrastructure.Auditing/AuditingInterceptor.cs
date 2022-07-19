using System.Text.Json;
using Infrastructure.Auditing;
using Infrastructure.Auditing.Models;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Infrastructure.Auditing.Extensions;
using Core.Application.Interfaces;

public class AuditingInterceptor : ISaveChangesInterceptor
{
    private readonly IAuthenticatedUserService _authenticatedUser;
    private readonly AuditDbContext _auditContext;
    private SaveChangesAudit _audit;

    public AuditingInterceptor(AuditDbContext auditContext, IAuthenticatedUserService authenticatedUser)
    {
        _auditContext = auditContext;
        _authenticatedUser = authenticatedUser;
    }

    #region SavingChanges
    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        _audit = CreateAudit(eventData.Context, _authenticatedUser.Username);

        _auditContext.Add(_audit);
        await _auditContext.SaveChangesAsync();

        return result;
    }

    public InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        _audit = CreateAudit(eventData.Context, _authenticatedUser.Username);

        _auditContext.Add(_audit);
        _auditContext.SaveChanges();

        return result;
    }
    #endregion

    #region SavedChanges
    public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {

        _auditContext.Attach(_audit);
        _audit.Succeeded = true;
        _audit.EndTime = DateTime.UtcNow;

        _auditContext.SaveChanges();

        return result;
    }

    public async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        _auditContext.Attach(_audit);
        _audit.Succeeded = true;
        _audit.EndTime = DateTime.UtcNow;

        await _auditContext.SaveChangesAsync(cancellationToken);

        return result;
    }
    #endregion

    #region SaveChangesFailed
    public void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        _auditContext.Attach(_audit);
        _audit.Succeeded = false;
        _audit.EndTime = DateTime.UtcNow;
        _audit.ErrorMessage = eventData.Exception.Message;

        _auditContext.SaveChanges();
    }

    public async Task SaveChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        _auditContext.Attach(_audit);
        _audit.Succeeded = false;
        _audit.EndTime = DateTime.UtcNow;
        _audit.ErrorMessage = eventData.Exception.InnerException?.Message;

        await _auditContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region CreateAudit
    private static SaveChangesAudit CreateAudit(DbContext context, string username)
    {
        context.ChangeTracker.DetectChanges();

        var audit = new SaveChangesAudit { AuditId = Guid.NewGuid(), StartTime = DateTime.UtcNow };
        var entries = context.ChangeTracker.Entries().Where(e => !AuditUtilities.IsAuditDisabled(e.Entity.GetType()) && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)).ToArray();

        foreach (var entry in entries)
        {
            var history = new AuditHistory
            {
                TableName = entry.Metadata.GetTableName(),
                Username = username
            };

            // Get the mapped properties for the entity type.
            // (include shadow properties, not include navigations & references)
            var properties = entry.Properties.Where(p => !AuditUtilities.IsAuditDisabled(p.EntityEntry.Entity.GetType(), p.Metadata.Name));

            foreach (var prop in properties)
            {
                string propertyName = prop.Metadata.Name;
                if (prop.Metadata.IsPrimaryKey())
                {
                    history.AutoHistoryDetails.NewValues[propertyName] = prop.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        history.PrimaryKey = "0";
                        history.Kind = EntityState.Added;
                        history.AutoHistoryDetails.NewValues.Add(propertyName, prop.CurrentValue);
                        break;

                    case EntityState.Modified:
                        history.PrimaryKey = entry.PrimaryKey();
                        history.Kind = EntityState.Modified;
                        history.AutoHistoryDetails.OldValues.Add(propertyName, prop.OriginalValue);
                        history.AutoHistoryDetails.NewValues.Add(propertyName, prop.CurrentValue);
                        break;

                    case EntityState.Deleted:
                        history.PrimaryKey = entry.PrimaryKey();
                        history.Kind = EntityState.Deleted;
                        history.AutoHistoryDetails.OldValues.Add(propertyName, prop.OriginalValue);
                        break;
                }
            }

            history.Changed = JsonSerializer.Serialize(history.AutoHistoryDetails);

            audit.AuditHistory.Add(history);
        }

        return audit;
    }

    #endregion
}

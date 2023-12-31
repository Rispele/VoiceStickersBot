using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace VoiceStickersBot.Infra.DatabaseTable;

public sealed class DatabaseTable<TEntity> : DbContext, ITable<TEntity>, ISchemaCreator
    where TEntity : class
{
    public DatabaseTable(DbContextOptions<DatabaseTable<TEntity>> options)
        : base(options)
    {
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    private DbSet<TEntity> Entities { get; set; } = null!;

    public async Task<bool> EnsureCreatedAsync()
    {
        var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        try
        {
            await dbCreator.CreateTablesAsync().ConfigureAwait(false);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task PerformCreateRequestAsync(
        TEntity entity,
        CancellationToken cancellationToken)
    {
        var entry = await Entities
            .AddAsync(entity, cancellationToken)
            .ConfigureAwait(false);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        entry.State = EntityState.Detached;
    }

    public async Task PerformUpdateRequestAsync(
        Func<IQueryable<TEntity>, TEntity> getter,
        Action<TEntity> updater,
        CancellationToken cancellationToken)
    {
        var entity = getter(Entities);
        var entry = Entities.Attach(entity);
        updater(entity);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        entry.State = EntityState.Detached;
    }

    public async Task<List<TExtractedEntity>> PerformReadonlyRequestAsync<TExtractedEntity>(
        Func<IQueryable<TEntity>, IQueryable<TExtractedEntity>> request,
        CancellationToken cancellationToken)
    {
        var entities = await request(Entities)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return entities;
    }

    public async Task<int> PerformDeletionRequestAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> request,
        CancellationToken cancellationToken)
    {
        var totalDeleted = await request(Entities)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return totalDeleted;
    }
}
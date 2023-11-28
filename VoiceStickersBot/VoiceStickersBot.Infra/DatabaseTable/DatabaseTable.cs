using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VoiceStickersBot.Infra.VsbDatabaseCluster;

public sealed class DatabaseTable<TEntity> : DbContext, ITable<TEntity>
    where TEntity : class
{
    public DatabaseTable(DbContextOptions<DatabaseTable<TEntity>> options)
        : base(options)
    {
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    private DbSet<TEntity> Entities { get; set; } = null!;

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
        TEntity entity,
        CancellationToken cancellationToken)
    {
        var entry = await Task<EntityEntry<TEntity>>
            .Factory
            .StartNew(() => Entities.Update(entity), cancellationToken)
            .ConfigureAwait(false);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        entry.State = EntityState.Detached;
    }

    public async Task<List<TEntity>> PerformReadonlyRequestAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> request,
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
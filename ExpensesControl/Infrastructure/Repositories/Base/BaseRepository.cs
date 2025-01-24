using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ExpensesControl.Infrastructure.SqlServer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Base
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class where TKey : struct
    {
        public readonly SqlContext _context;
        public readonly ILogger<BaseRepository<TEntity, TKey>> _logger;

        /// <summary>
        /// Initializes a new instance of the GenericRepository class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="logger">The logger instance.</param>
        protected BaseRepository(SqlContext context, ILogger<BaseRepository<TEntity, TKey>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new entity.");
            EntityEntry<TEntity> ret = await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            _logger.LogInformation("Entity created successfully.");
            return ret.Entity;
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        public virtual async Task<int> UpdateAsync(TEntity entity, TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating an entity.");

            // Check if the entity exists in the database
            var existingEntity = await _context.Set<TEntity>().FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Entidade não encontrada.");
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            _logger.LogInformation("Entity updated successfully.");
            return 1; // Return 1 to indicate success
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        public virtual async Task<bool> DeleteAsync(TEntity entity, TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting an entity.");

            // Check if the entity exists in the database
            var existingEntity = await _context.Set<TEntity>().FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Entidade não encontrada.");
            EntityEntry<TEntity>? entry = _context.Entry(existingEntity);
            entry.State = EntityState.Deleted;

            _logger.LogInformation("Entity deleted successfully.");
            return true;
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        public virtual TEntity GetById(TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving an entity by ID: {Id}.", id);
            TEntity? entity = _context.Set<TEntity>().Find(id, cancellationToken);
            if (entity is null)
            {
                _logger.LogWarning("Entity not found for ID: {Id}.", id);
                throw new KeyNotFoundException($"Entidade não encontrada para o ID: {id}");
            }
            else
            {
                _logger.LogInformation("Entity retrieved successfully for ID: {Id}.", id);
            }
            return entity;
        }

        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <param name="include">The include expression for related entities.</param>
        /// <returns>The retrieved entity, including its related entities.</returns>
        public virtual async Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving an entity asynchronously by ID: {Id}.", id);

            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            TEntity entity = await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id), cancellationToken)
                        ?? throw new KeyNotFoundException($"Entidade não encontrada para o ID: {id}");

            _logger.LogInformation("Entity retrieved successfully for ID: {Id}.", id);
            return entity;
        }

        /// <summary>
        /// Retrieves entities based on a specification.
        /// </summary>
        public async Task<IEnumerable<TEntity>> ListBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving entities based on a specification.");
            var specificationResult = ApplySpecification(specification);
            return await specificationResult.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a single entity based on a specification.
        /// </summary>
        public async Task<TEntity?> GetSingleBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving a single entity asynchronously based on a specification.");
            var specificationResult = ApplySpecification(specification);

            return await specificationResult.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves entities based on a specification.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator.Default.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
        }
    }
}

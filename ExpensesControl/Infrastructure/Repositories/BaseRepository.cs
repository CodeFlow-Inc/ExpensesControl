using ExpensesControl.Infrastructure.Persistence;
using ExpensesControl.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Infrastructure.Repositories
{
    public class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T : class where TKey : struct
    {
        private readonly SqlContext _context;
        private readonly ILogger<BaseRepository<T, TKey>> _logger;

        /// <summary>
        /// Initializes a new instance of the GenericRepository class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="logger">The logger instance.</param>
        protected BaseRepository(SqlContext context, ILogger<BaseRepository<T, TKey>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        public virtual async Task<T> CreateAsync(T entity)
        {
            _logger.LogInformation("Creating a new entity.");
            EntityEntry<T> ret = _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Entity created successfully.");
            return ret.Entity;
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        public virtual async Task<int> UpdateAsync(T entity, TKey id)
        {
            _logger.LogInformation("Updating an entity.");

            // Check if the entity exists in the database
            var existingEntity = await _context.Set<T>().FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException("Entity not found");
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            int result = await _context.SaveChangesAsync();

            _logger.LogInformation("Entity updated successfully.");
            return result;
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        public virtual async Task<bool> DeleteAsync(T entity, TKey id)
        {
            _logger.LogInformation("Deleting an entity.");

            // Check if the entity exists in the database
            var existingEntity = await _context.Set<T>().FindAsync(id) ?? throw new KeyNotFoundException("Entity not found");
            EntityEntry<T>? entry = _context.Entry(existingEntity);
            entry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Entity deleted successfully.");
            return true;
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        public virtual T GetById(TKey id)
        {
            _logger.LogInformation("Retrieving an entity by ID: {Id}.", id);
            T entity = _context.Set<T>().Find(id)!;
            if (entity == null)
            {
                _logger.LogWarning("Entity not found for ID: {Id}.", id);
                throw new KeyNotFoundException($"Entity not found for ID: {id}");
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
        public virtual async Task<T> GetByIdAsync(TKey id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            _logger.LogInformation("Retrieving an entity asynchronously by ID: {Id}.", id);

            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            T entity = await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id))
                        ?? throw new KeyNotFoundException($"Entity not found for ID: {id}");

            _logger.LogInformation("Entity retrieved successfully for ID: {Id}.", id);
            return entity;
        }

        /// <summary>
        /// Returns a queryable collection of all entities.
        /// </summary>
        public virtual IQueryable<T> ListAll()
        {
            _logger.LogInformation("Listing all entities.");
            return _context.Set<T>().AsQueryable();
        }
    }
}

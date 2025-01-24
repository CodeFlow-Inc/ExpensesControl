﻿using ExpensesControl.Infrastructure.SqlServer.Persistence;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Base
{
    public class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T : class where TKey : struct
    {
        public readonly SqlContext _context;
        public readonly ILogger<BaseRepository<T, TKey>> _logger;

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
        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new entity.");
            EntityEntry<T> ret = await _context.Set<T>().AddAsync(entity, cancellationToken);
            _logger.LogInformation("Entity created successfully.");
            return ret.Entity;
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        public virtual async Task<int> UpdateAsync(T entity, TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating an entity.");

            // Check if the entity exists in the database
            var existingEntity = await _context.Set<T>().FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Entidade não encontrada.");
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            _logger.LogInformation("Entity updated successfully.");
            return 1; // Return 1 to indicate success
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        public virtual async Task<bool> DeleteAsync(T entity, TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting an entity.");

            // Check if the entity exists in the database
            var existingEntity = await _context.Set<T>().FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Entidade não encontrada.");
            EntityEntry<T>? entry = _context.Entry(existingEntity);
            entry.State = EntityState.Deleted;

            _logger.LogInformation("Entity deleted successfully.");
            return true;
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        public virtual T GetById(TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving an entity by ID: {Id}.", id);
            T? entity = _context.Set<T>().Find(id, cancellationToken);
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
        public virtual async Task<T> GetByIdAsync(TKey id, Func<IQueryable<T>, IQueryable<T>>? include = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving an entity asynchronously by ID: {Id}.", id);

            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            T entity = await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id), cancellationToken)
                        ?? throw new KeyNotFoundException($"Entidade não encontrada para o ID: {id}");

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

        /// <summary>
        /// Retrieves entities based on a specification.
        /// </summary>
        public virtual IQueryable<T> ListBySpecification(ISpecification<T> specification)
        {
            _logger.LogInformation("Retrieving entities based on a specification.");
            return specification.Apply(_context.Set<T>().AsQueryable());
        }

        /// <summary>
        /// Retrieves entities based on a specification asynchronously.
        /// </summary>
        public virtual async Task<List<T>> ListBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving entities asynchronously based on a specification.");
            return await specification.Apply(_context.Set<T>().AsQueryable()).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a single entity based on a specification.
        /// </summary>
        public virtual async Task<T?> GetSingleBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving a single entity asynchronously based on a specification.");
            return await specification.Apply(_context.Set<T>().AsQueryable()).FirstOrDefaultAsync(cancellationToken);
        }
    }
}

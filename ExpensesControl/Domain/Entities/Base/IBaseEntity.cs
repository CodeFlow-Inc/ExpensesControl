namespace ExpensesControl.Domain.Entities.Base
{
    /// <summary>
    /// Represents the base contract for an entity with a primary key and audit fields.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public interface IBaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// Gets the date and time when the entity was created.
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// Gets the username of the user who created the entity.
        /// </summary>
        string CreatedByUser { get; }

        /// <summary>
        /// Gets the username of the user who last updated the entity.
        /// </summary>
        string UpdatedByUser { get; }

        /// <summary>
        /// Gets the date and time when the entity was last updated.
        /// </summary>
        DateTime LastUpdateDate { get; }

        /// <summary>
        /// Marks the entity as newly created, initializing the appropriate fields.
        /// </summary>
        void MarkAsCreated();

        /// <summary>
        /// Marks the entity as updated, updating the appropriate fields.
        /// </summary>
        void MarkAsUpdated();

        /// <summary>
        /// Sets the username for the user responsible for the current operation.
        /// </summary>
        /// <param name="user">The username of the current user.</param>
        /// <returns>The updated entity.</returns>
        BaseEntity<TKey> SetCurrentUser(string user);
    }
}

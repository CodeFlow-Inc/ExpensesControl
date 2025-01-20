using Destructurama.Attributed;

namespace ExpensesControl.Domain.Entities.Base
{
    /// <summary>
    /// Represents the base implementation of an entity with audit fields.
    /// </summary>
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        public required TKey Id { get; set; }

        /// <summary>
        /// Gets the date and time when the entity was created.
        /// </summary>
        [NotLogged]
        public DateTime CreationDate { get; protected set; }

        /// <summary>
        /// Gets the username of the user who created the entity.
        /// </summary>
        [NotLogged]
        public string CreatedByUser { get; protected set; }

        /// <summary>
        /// Gets the date and time when the entity was last updated.
        /// </summary>
        [NotLogged]
        public DateTime LastUpdateDate { get; protected set; }

        /// <summary>
        /// Gets the username of the user who last updated the entity.
        /// </summary>
        [NotLogged]
        public string UpdatedByUser { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity{TKey}"/> class, marking the entity as created.
        /// </summary>
        protected BaseEntity()
        {
            MarkAsCreated();
        }

        /// <summary>
        /// Marks the entity as created, initializing the creation and update timestamps.
        /// </summary>
        public void MarkAsCreated()
        {
            DateTime dateTime = DateTime.Now;
            CreationDate = dateTime;
            LastUpdateDate = dateTime;
        }

        /// <summary>
        /// Sets the user responsible for the creation and/or update of the entity.
        /// </summary>
        /// <param name="user">The username of the current user.</param>
        /// <returns>The updated entity.</returns>
        public BaseEntity<TKey> SetCurrentUser(string user)
        {
            if (string.IsNullOrEmpty(CreatedByUser))
                CreatedByUser = user;
            UpdatedByUser = user;
            return this;
        }

        /// <summary>
        /// Marks the entity as updated, updating the update timestamp.
        /// </summary>
        public void MarkAsUpdated()
        {
            LastUpdateDate = DateTime.Now;
        }
    }
}

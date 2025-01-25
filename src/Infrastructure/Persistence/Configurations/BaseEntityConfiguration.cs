using CodeFlow.Start.Lib.Context.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence.Configurations;

/// <summary>
/// Base configuration for entities inheriting from <see cref="BaseEntity{TKey}"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class BaseEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : BaseEntity<TKey>
{
	/// <summary>
	/// Configures the entity properties.
	/// </summary>
	/// <param name="builder">The entity builder used for configuration.</param>
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		// Configures the primary key
		builder.HasKey(e => e.Id);

		// Configures the CreationDate property
		builder.Property(e => e.CreationDate)
			.IsRequired()
			.HasColumnType("datetime")
			.HasColumnName("creation_date");

		// Configures the CreatedByUser property
		builder.Property(e => e.CreatedByUser)
			.IsRequired()
			.HasMaxLength(100)
			.HasColumnName("created_by_user");

		// Configures the LastUpdateDate property
		builder.Property(e => e.LastUpdateDate)
			.IsRequired()
			.HasColumnType("datetime")
			.HasColumnName("last_update_date");

		// Configures the UpdatedByUser property
		builder.Property(e => e.UpdatedByUser)
			.IsRequired()
			.HasMaxLength(100)
			.HasColumnName("updated_by_user");
	}
}

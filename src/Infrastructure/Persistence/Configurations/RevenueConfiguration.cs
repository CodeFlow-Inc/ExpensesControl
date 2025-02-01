using CodeFlow.Data.Context.Package.Configuration;
using ExpensesControl.Domain.Entities.AggregateRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence.Configurations;

/// <summary>
/// Configuration for the Revenue entity.
/// </summary>
public class RevenueConfiguration : BaseEntityConfiguration<Revenue, int>
{
	/// <summary>
	/// Configures the Revenue entity.
	/// </summary>
	/// <param name="builder">The builder to configure the entity.</param>
	public override void Configure(EntityTypeBuilder<Revenue> builder)
	{
		// Calls the base configuration (BaseEntityConfiguration)
		base.Configure(builder);

		// Table configuration
		builder.ToTable("revenues");

		// UserCode (required)
		builder.Property(r => r.UserCode)
			.IsRequired()
			.HasColumnName("user_code");

		// Description (optional, max length)
		builder.Property(r => r.Description)
			.HasMaxLength(500)
			.IsRequired(false)
			.HasColumnName("description");

		// Amount (required, precision for monetary values)
		builder.Property(r => r.Amount)
			.IsRequired()
			.HasPrecision(18, 2) // Defines precision for monetary values
			.HasColumnName("amount");

		// StartDate (required)
		builder.Property(e => e.StartDate)
			.IsRequired()
			.HasColumnType("date") // Maps to SQL "date" type
			.HasColumnName("start_date");

		// EndDate (optional)
		builder.Property(e => e.EndDate)
			.HasColumnType("date") // Maps to SQL "date" type
			.HasColumnName("end_date");

		// Type (required enum)
		builder.Property(r => r.Type)
			.IsRequired()
			.HasColumnName("type");

		// Configuration for the Value Object: Recurrence
		builder.OwnsOne(r => r.Recurrence, recurrence =>
		{
			recurrence.Property(rc => rc.IsRecurring)
				.IsRequired()
				.HasColumnName("is_recurring");

			recurrence.Property(rc => rc.Periodicity)
				.IsRequired()
				.HasColumnName("recurrence_periodicity");

			recurrence.Property(rc => rc.MaxOccurrences)
				.HasColumnName("max_occurrences");
		});
	}
}

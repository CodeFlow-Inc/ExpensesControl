using ExpensesControl.Domain.Entities.AggregateRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence.Configurations;

/// <summary>
/// Configuration for the Expense entity.
/// </summary>
public class ExpenseConfiguration : BaseEntityConfiguration<Expense, int>
{
    /// <summary>
    /// Configures the Expense entity.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public override void Configure(EntityTypeBuilder<Expense> builder)
    {
        // Calls the base configuration (BaseEntityConfiguration)
        base.Configure(builder);

        // Table configuration
        builder.ToTable("expenses");

        // UserCode (required)
        builder.Property(e => e.UserCode)
            .IsRequired()
            .HasColumnName("user_code");

        // Description (optional, max length)
        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired(false)
            .HasColumnName("description");

        // Value (required)
        builder.Property(e => e.Value)
            .IsRequired()
            .HasPrecision(18, 2) // Defines precision for monetary values
            .HasColumnName("value");

        // StartDate (required)
        builder.Property(e => e.StartDate)
            .IsRequired()
            .HasColumnType("date") // Maps to SQL "date" type
            .HasColumnName("start_date");

        // EndDate (optional)
        builder.Property(e => e.EndDate)
            .HasColumnType("date") // Maps to SQL "date" type
            .HasColumnName("end_date");

        // Category (required enum)
        builder.Property(e => e.Category)
            .IsRequired()
            .HasColumnName("category");

        // Notes (optional, max length)
        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false)
            .HasColumnName("notes");

        // Configuration for the Value Object: PaymentMethod
        builder.OwnsOne(e => e.PaymentMethod, paymentMethod =>
        {
            paymentMethod.Property(p => p.Type)
                .IsRequired()
                .HasColumnName("payment_method_type");

            paymentMethod.Property(p => p.IsInstallment)
                .IsRequired()
                .HasColumnName("is_installment");

            paymentMethod.Property(p => p.InstallmentCount)
                .HasColumnName("installment_count");

            paymentMethod.Property(p => p.InstallmentValue)
                .HasPrecision(18, 2) // Defines precision for monetary values
                .IsRequired(false)
                .HasColumnName("installment_value");

            paymentMethod.Property(p => p.Notes)
                .HasMaxLength(500)
                .HasColumnName("payment_notes");
        });

        // Configuration for the Value Object: Recurrence
        builder.OwnsOne(e => e.Recurrence, recurring =>
        {
            recurring.Property(r => r.IsRecurring)
                .IsRequired()
                .HasColumnName("is_recurring");

            recurring.Property(r => r.Periodicity)
                .IsRequired()
                .HasColumnName("recurrence_periodicity");

            recurring.Property(r => r.MaxOccurrences)
                .HasColumnName("max_occurrences");
        });
    }
}

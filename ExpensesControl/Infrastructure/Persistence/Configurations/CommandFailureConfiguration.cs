using ExpensesControl.Domain.Entities.Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesControl.Infrastructure.SqlServer.Persistence.Configurations;

/// <summary>
/// Configuration for the CommandFailure entity.
/// </summary>
public class CommandFailureConfiguration : IEntityTypeConfiguration<CommandFailure>
{
    /// <summary>
    /// Configures the CommandFailure entity.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<CommandFailure> builder)
    {
        // Table configuration
        builder.ToTable("command_failure");

        // Configures the primary key
        builder.HasKey(e => e.Id);

        // Property configurations
        builder.Property(cf => cf.CommandName)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("command_name");

        builder.Property(cf => cf.ErrorDetails)
            .IsRequired()
            .HasColumnName("error_details");

        builder.Property(cf => cf.Timestamp)
            .IsRequired()
            .HasColumnName("timestamp");

        builder.Property(cf => cf.RequestContent)
            .HasMaxLength(4000)
            .HasColumnName("request_content");

        builder.Property(cf => cf.TraceId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("trace_id");

        // Indexes
        builder.HasIndex(cf => cf.CommandName)
            .HasDatabaseName("idx_command_name");

        builder.HasIndex(cf => cf.TraceId)
            .HasDatabaseName("idx_trace_id");
    }
}

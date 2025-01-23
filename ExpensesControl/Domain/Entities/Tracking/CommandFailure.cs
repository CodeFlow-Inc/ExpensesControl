using ExpensesControl.Domain.Entities.Base;

namespace ExpensesControl.Domain.Entities.Tracking;

/// <summary>
/// Represents a record of a failed command execution.
/// </summary>
public class CommandFailure
{
    /// <summary>
    /// Gets or sets the unique identifier of the CommandFailure.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Name of the command that failed.
    /// </summary>
    public required string CommandName { get; set; }

    /// <summary>
    /// Details of the error that occurred.
    /// </summary>
    public required string ErrorDetails { get; set; }

    /// <summary>
    /// Timestamp of when the failure occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// The raw request content that caused the failure (optional for debugging purposes).
    /// </summary>
    public string? RequestContent { get; set; }

    /// <summary>
    /// Gets or sets the trace identifier for the request, used for tracking and correlation purposes.
    /// </summary>
    public string? TraceId { get; set; }
}

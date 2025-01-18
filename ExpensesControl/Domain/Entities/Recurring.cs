namespace ExpensesControl.Domain.Entities;

/// <summary>
/// Represents recurrence details for an expense.
/// </summary>
public class Recurring
{
    /// <summary>
    /// Indicates whether the expense is recurring.
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Periodicity of the recurrence (e.g., daily, weekly, monthly, yearly).
    /// </summary>
    public RecurrencePeriodicity Periodicity { get; set; }

    /// <summary>
    /// Maximum number of occurrences for the recurrence.
    /// If null, the recurrence is indefinite.
    /// </summary>
    public int? MaxOccurrences { get; set; }

    /// <summary>
    /// Validates the consistency of the recurrence data.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when validation fails.</exception>
    public void Validate()
    {
        if (IsRecurring)
        {
            if (MaxOccurrences.HasValue && MaxOccurrences <= 0)
            {
                throw new InvalidOperationException("O número máximo de ocorrências deve ser maior que zero.");
            }
        }
        else
        {
            if (MaxOccurrences.HasValue)
            {
                throw new InvalidOperationException("Despesas não recorrentes não devem ter número máximo de ocorrências.");
            }
        }
    }
}

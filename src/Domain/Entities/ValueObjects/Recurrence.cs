using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Domain.Entities.ValueObjects;

/// <summary>
/// Represents recurrence details for an expense.
/// </summary>
public class Recurrence
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
	/// Validates the rules related to the recurrence of an expense, adding errors to the output parameter.
	/// </summary>
	/// <param name="errors">A list of error messages returned if any validation rule is violated.</param>
	/// <returns>
	/// Returns <c>true</c> if the validation passes without errors; otherwise, <c>false</c>.
	/// </returns>
	public bool Validate(out List<string> errors)
	{
		errors = [];

		if (IsRecurring)
		{
			if (MaxOccurrences.HasValue && MaxOccurrences <= 0)
			{
				errors.Add("O número máximo de ocorrências deve ser maior que zero.");
			}
		}
		else
		{
			if (MaxOccurrences.HasValue)
			{
				errors.Add("Despesas não recorrentes não devem ter número máximo de ocorrências.");
			}
		}

		return errors.Count == 0;
	}

}

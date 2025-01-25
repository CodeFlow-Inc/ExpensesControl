using CodeFlow.Start.Lib.Context.Base;
using Destructurama.Attributed;
using ExpensesControl.Domain.Entities.ValueObjects;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Domain.Entities.AggregateRoot;

/// <summary>
/// Represents an expense entry.
/// </summary>
public class Expense : BaseEntity<int>
{
	/// <summary>
	/// User code associated with the expense.
	/// </summary>
	[LogMasked]
	public int UserCode { get; set; }

	/// <summary>
	/// Description of the expense.
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// Start date when the expense was incurred.
	/// </summary>
	public DateOnly StartDate { get; set; }

	/// <summary>
	/// End date for the expense (nullable).
	/// If the expense is recurring, this can be null to indicate an indefinite period.
	/// Otherwise, it is equal to StartDate.
	/// </summary>
	public DateOnly? EndDate { get; set; }

	/// <summary>
	/// Category of the expense.
	/// </summary>
	public ExpenseCategory Category { get; set; }

	/// <summary>
	/// Recurrence details for the expense.
	/// </summary>
	public Recurrence Recurrence { get; set; } = new Recurrence();

	/// <summary>
	/// Payment for the expense.
	/// </summary>
	public Payment Payment { get; set; } = new Payment();

	/// <summary>
	/// Additional notes or details about the expense.
	/// </summary>
	public string? Notes { get; set; }

	/// <summary>
	/// Validates the consistency of expense data by checking specific business rules.
	/// </summary>
	/// <param name="errors">
	/// A list of error messages returned if any validation rule is violated.
	/// </param>
	/// <returns>
	/// Returns <c>true</c> if the validation passes without errors; otherwise, <c>false</c>.
	/// </returns>
	public bool Validate(out List<string> errors)
	{
		errors = [];

		if (Recurrence.IsRecurring)
		{
			if (EndDate.HasValue && EndDate < StartDate)
			{
				errors.Add("A data final não pode ser anterior à data inicial.");
			}
		}
		else
		{
			if (EndDate.HasValue && EndDate != StartDate)
			{
				errors.Add("Para despesas não recorrentes, a data final deve ser igual à data inicial.");
			}

			EndDate = StartDate;
		}

		if (UserCode <= 0)
		{
			errors.Add("O código do usuário deve ser um número inteiro positivo.");
		}

		if (!Recurrence.Validate(out var errorsRecurrence))
			errors.AddRange(errorsRecurrence);

		if (!Payment.Validate(out var errorsPaymentMethod))
			errors.AddRange(errorsPaymentMethod);

		return errors.Count == 0;
	}
}

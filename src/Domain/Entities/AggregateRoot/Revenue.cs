using CodeFlow.Data.Context.Package.Base.Entities;
using Destructurama.Attributed;
using ExpensesControl.Domain.Entities.ValueObjects;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Domain.Entities.AggregateRoot;

/// <summary>
/// Represents an Revenue entry.
/// </summary>
public class Revenue : BaseEntity<int>
{
	/// <summary>
	/// User code associated with the expense.
	/// </summary>
	[LogMasked]
	public int UserCode { get; set; }

	/// <summary>
	/// Description of the Revenue
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// Amount of the Revenue
	/// </summary>
	public decimal Amount { get; set; }

	/// <summary>
	/// Date of the Revenue
	/// </summary>
	public DateOnly StartDate { get; set; }

	/// <summary>
	/// Date of the Revenue	
	/// </summary>
	public DateOnly? EndDate { get; set; }

	/// <summary>
	/// Type of the Revenue
	/// </summary>
	public TypeRevenue Type { get; set; }

	/// <summary>
	/// Recurrence of Revenue
	/// </summary>
	public Recurrence Recurrence { get; set; } = new Recurrence();

	/// <summary>
	/// Initializes a new instance of the <see cref="Revenue"/> class.
	/// </summary>
	/// <param name="errors"></param>
	/// <returns></returns>
	public bool Validate(out List<string> errors)
	{
		errors = [];

		if (Amount <= 0)
		{
			errors.Add("O valor deve ser maior que zero.");
		}

		if (UserCode <= 0)
		{
			errors.Add("O código do usuário deve ser um número inteiro positivo.");
		}

		if (!Recurrence.Validate(out var errorsRecurrence))
			errors.AddRange(errorsRecurrence);

		return errors.Count == 0;
	}
}

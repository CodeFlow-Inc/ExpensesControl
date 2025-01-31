using Ardalis.Specification;
using ExpensesControl.Domain.Entities.AggregateRoot;

namespace ExpensesControl.Application.Specs;

/// <summary>
/// Specification for querying expenses with various filtering options.
/// </summary>
public class ExpenseSpec : Specification<Expense>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ExpenseSpec"/> class with optional tracking.
	/// </summary>
	/// <param name="noTracking">Indicates whether tracking should be disabled.</param>
	public ExpenseSpec(bool noTracking = true)
	{
		Query.AsNoTracking(noTracking);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ExpenseSpec"/> class for a specific user.
	/// </summary>
	/// <param name="userCode">The unique identifier of the user.</param>
	/// <param name="noTracking">Indicates whether tracking should be disabled.</param>
	public ExpenseSpec(int userCode, bool noTracking = true) : this(noTracking)
	{
		IncludePayment();
		IncludeRecurrence();
		Query.Where(e => e.UserCode == userCode)
			 .OrderBy(e => e.Category)
			 .ThenBy(e => e.Payment.TotalValue);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ExpenseSpec"/> class for a specific user and month/year.
	/// </summary>
	/// <param name="userCode">The unique identifier of the user.</param>
	/// <param name="month">The target month.</param>
	/// <param name="year">The target year.</param>
	/// <param name="noTracking">Indicates whether tracking should be disabled.</param>
	public ExpenseSpec(int userCode, int month, int year, bool noTracking = true) : this(noTracking)
	{
		IncludePayment();
		IncludeRecurrence();

		var targetDate = new DateOnly(year, month, 1);
		var firstDayOfMonth = targetDate;
		var lastDayOfMonth = targetDate.AddMonths(1).AddDays(-1);

		Query.Where(e => e.UserCode == userCode &&
			(
				// Case 1: Non-recurring expenses in the given month/year
				(!e.Recurrence.IsRecurring &&
				 e.StartDate.Month == month &&
				 e.StartDate.Year == year) ||

				// Case 2: Recurring expenses active during the given month/year
				(e.Recurrence.IsRecurring &&
				 e.StartDate <= lastDayOfMonth &&
				 (e.EndDate == null || e.EndDate >= firstDayOfMonth))
			))
			.OrderBy(e => e.StartDate);
	}

	/// <summary>
	/// Includes the payment details in the query.
	/// </summary>
	/// <returns>The updated <see cref="ExpenseSpec"/> instance.</returns>
	public ExpenseSpec IncludePayment()
	{
		Query.Include(e => e.Payment);
		return this;
	}

	/// <summary>
	/// Includes the recurrence details in the query.
	/// </summary>
	/// <returns>The updated <see cref="ExpenseSpec"/> instance.</returns>
	public ExpenseSpec IncludeRecurrence()
	{
		Query.Include(e => e.Recurrence);
		return this;
	}
}
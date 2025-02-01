using Ardalis.Specification;
using ExpensesControl.Domain.Entities.AggregateRoot;

namespace ExpensesControl.Application.Specs;

/// <summary>
/// Specification for querying Revenue entities.
/// </summary>
public class RevenueSpec : Specification<Revenue>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RevenueSpec"/> class.
	/// </summary>
	/// <param name="noTracking">Indicates whether tracking should be disabled.</param>
	public RevenueSpec(bool noTracking = true)
	{
		Query.AsNoTracking(noTracking);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RevenueSpec"/> class filtered by user.
	/// </summary>
	/// <param name="userCode">The user code to filter revenues.</param>
	/// <param name="noTracking">Indicates whether tracking should be disabled.</param>
	public RevenueSpec(int userCode, bool noTracking = true) : this(noTracking)
	{
		IncludeRecurrence();
		Query.Where(r => r.UserCode == userCode)
			 .OrderBy(r => r.StartDate);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RevenueSpec"/> class for monthly reports.
	/// </summary>
	/// <param name="userCode">The user code to filter revenues.</param>
	/// <param name="month">The month for the report.</param>
	/// <param name="year">The year for the report.</param>
	/// <param name="noTracking">Indicates whether tracking should be disabled.</param>
	public RevenueSpec(int userCode, int month, int year, bool noTracking = true) : this(noTracking)
	{
		IncludeRecurrence();

		var targetDate = new DateOnly(year, month, 1);
		var firstDayOfMonth = targetDate;
		var lastDayOfMonth = targetDate.AddMonths(1).AddDays(-1);

		Query.Where(r => r.UserCode == userCode &&
			(
				// Case 1: Non-recurring revenues within the specified month/year
				(!r.Recurrence.IsRecurring &&
				 r.StartDate.Month == month &&
				 r.StartDate.Year == year) ||

				// Case 2: Recurring revenues active during the specified month/year
				(r.Recurrence.IsRecurring &&
				 r.StartDate <= lastDayOfMonth &&
				 (r.EndDate == null || r.EndDate >= firstDayOfMonth))
			))
			.OrderBy(r => r.StartDate);
	}

	/// <summary>
	/// Includes the Recurrence entity in the query.
	/// </summary>
	/// <returns>The updated <see cref="RevenueSpec"/> instance.</returns>
	public RevenueSpec IncludeRecurrence()
	{
		Query.Include(r => r.Recurrence);
		return this;
	}
}

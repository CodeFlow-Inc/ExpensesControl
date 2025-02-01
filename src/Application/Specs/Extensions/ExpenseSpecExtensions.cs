using Ardalis.Specification;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.Specs.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="ExpenseSpec"/> specification.
/// </summary>
public static class ExpenseSpecExtensions
{
	/// <summary>
	/// Filters expenses by the specified category.
	/// </summary>
	/// <param name="spec">The existing <see cref="ExpenseSpec"/> instance.</param>
	/// <param name="category">The expense category to filter by.</param>
	/// <returns>The modified <see cref="ExpenseSpec"/> instance with the category filter applied.</returns>
	public static ExpenseSpec ForCategory(
		this ExpenseSpec spec,
		ExpenseCategory category)
	{
		spec.Query.Where(e => e.Category == category);
		return spec;
	}
}

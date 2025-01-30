using Ardalis.Specification;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.Specs.Extensions;
public static class ExpenseSpecExtensions
{
	public static ExpenseSpec ForCategory(
		this ExpenseSpec spec,
		ExpenseCategory category)
	{
		spec.Query.Where(e => e.Category == category);
		return spec;
	}
}

using Ardalis.Specification;
using ExpensesControl.Domain.Entities.AggregateRoot;

namespace ExpensesControl.Application.Specs;

public class RevenueSpec : Specification<Revenue>
{
	public RevenueSpec(bool noTracking = true)
	{
		Query.AsNoTracking(noTracking);
	}

	// Construtor para consulta por usuário
	public RevenueSpec(int userCode, bool noTracking = true) : this(noTracking)
	{
		IncludeRecurrence();
		Query.Where(r => r.UserCode == userCode)
			 .OrderBy(r => r.StartDate);
	}

	// Construtor para relatório mensal
	public RevenueSpec(int userCode, int month, int year, bool noTracking = true) : this(noTracking)
	{
		IncludeRecurrence();

		var targetDate = new DateOnly(year, month, 1);
		var firstDayOfMonth = targetDate;
		var lastDayOfMonth = targetDate.AddMonths(1).AddDays(-1);

		Query.Where(r => r.UserCode == userCode &&
			(
				// Caso 1: Receitas não recorrentes no mês/ano
				(!r.Recurrence.IsRecurring &&
				 r.StartDate.Month == month &&
				 r.StartDate.Year == year) ||

				// Caso 2: Receitas recorrentes ativas durante o mês/ano
				(r.Recurrence.IsRecurring &&
				 r.StartDate <= lastDayOfMonth &&
				 (r.EndDate == null || r.EndDate >= firstDayOfMonth))
			))
			.OrderBy(r => r.StartDate);
	}

	public RevenueSpec IncludeRecurrence()
	{
		Query.Include(r => r.Recurrence);
		return this;
	}
}
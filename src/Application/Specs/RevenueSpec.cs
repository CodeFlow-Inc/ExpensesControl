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
			 .OrderBy(r => r.ReceiptDate);
	}

	// Construtor para relatório mensal
	public RevenueSpec(int userCode, int month, int year, bool noTracking = true) : this(noTracking)
	{
		IncludeRecurrence();
		Query.Where(r => r.UserCode == userCode &&
						 r.ReceiptDate.Month == month &&
						 r.ReceiptDate.Year == year)
			 .OrderBy(r => r.ReceiptDate);
	}

	public RevenueSpec IncludeRecurrence()
	{
		Query.Include(r => r.Recurrence);
		return this;
	}
}
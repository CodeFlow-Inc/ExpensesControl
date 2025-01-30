﻿using Ardalis.Specification;
using ExpensesControl.Domain.Entities.AggregateRoot;

namespace ExpensesControl.Application.Specs;

public class ExpenseSpec : Specification<Expense>
{
	public ExpenseSpec(bool noTracking = true)
	{
		Query.AsNoTracking(noTracking);
	}

	public ExpenseSpec(int userCode, bool noTracking = true) : this(noTracking)
	{
		IncludePayment();
		IncludeRecurrence();
		Query.Where(e => e.UserCode == userCode)
			 .OrderBy(e => e.Category)
			 .ThenBy(e => e.Payment.TotalValue);
	}

	public ExpenseSpec(int userCode, int month, int year, bool noTracking = true) : this(noTracking)
	{
		IncludePayment();
		IncludeRecurrence();
		Query.Where(e => e.UserCode == userCode &&
						 e.StartDate.Month == month &&
						 e.StartDate.Year == year)
			 .OrderBy(e => e.StartDate);
	}

	public ExpenseSpec IncludePayment()
	{
		Query.Include(e => e.Payment);
		return this;
	}

	public ExpenseSpec IncludeRecurrence()
	{
		Query.Include(e => e.Recurrence);
		return this;
	}
}
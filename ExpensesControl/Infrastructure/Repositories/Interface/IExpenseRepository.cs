using ExpensesControl.Domain.Entities;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;

public interface IExpenseRepository : IBaseRepository<Expense, int>
{
}

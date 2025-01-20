using ExpensesControl.Domain.Entities.AggregateRoot;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;

public interface IExpenseRepository : IBaseRepository<Expense, int>
{
}

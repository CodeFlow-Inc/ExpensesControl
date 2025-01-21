using ExpensesControl.Domain.Entities.AggregateRoot;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface
{
    public interface IIncomeRepository : IBaseRepository<Income, int>
    {
    }
}

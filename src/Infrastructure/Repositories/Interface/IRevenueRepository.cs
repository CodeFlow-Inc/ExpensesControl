using CodeFlow.Start.Lib.Context.Base.Repositories;
using ExpensesControl.Domain.Entities.AggregateRoot;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;

public interface IRevenueRepository : IBaseRepository<Revenue, int>
{
}

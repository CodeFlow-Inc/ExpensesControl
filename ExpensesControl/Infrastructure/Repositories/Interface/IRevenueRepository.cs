﻿using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Base;

namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface
{
    public interface IRevenueRepository : IBaseRepository<Revenue, int>
    {
    }
}

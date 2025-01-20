namespace ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;

/// <summary>
/// Interface for specifications.
/// </summary>
public interface ISpecification<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}

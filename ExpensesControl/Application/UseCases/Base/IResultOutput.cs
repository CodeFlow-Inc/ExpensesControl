namespace ExpensesControl.Application.UseCases.Base;

public interface IResultOutput<T>
{
    public T? Result { get; set; }

    /// <summary>
    /// Sets the result of the use case.
    /// </summary>
    /// <param name="result">The result returned by the use case.</param>
    public void SetResult(T result)
    {
        Result = result;
    }
}

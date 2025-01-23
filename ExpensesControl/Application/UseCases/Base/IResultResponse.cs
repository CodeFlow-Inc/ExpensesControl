namespace ExpensesControl.Application.UseCases.Base;

public interface IResultResponse<T>
{
    public T? Result { get; set; }
    public bool IsSuccess { get; }

    /// <summary>
    /// Sets the result of the use case.
    /// </summary>
    /// <param name="result">The result returned by the use case.</param>
    public void SetResult(T result)
    {
        Result = result;
    }
}

using ExpensesControl.Application.UseCases.Base;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto;

/// <summary>
/// Response after creating an expense.
/// </summary>
public class CreateExpenseOutput : BaseOutput, IResultOutput<CreateOutput>
{
    /// <summary>
    /// The output value of the use case.
    /// </summary>
    public CreateOutput? Result { get; set; } = default;

    /// <summary>
    /// Sets the result of the use case.
    /// </summary>
    /// <param name="result">The result returned by the use case.</param>
    public void SetResult(CreateOutput result)
    {
        Result = result;
    }
}

public class CreateOutput(int id)
{
    public int Id { get; set; } = id;
}

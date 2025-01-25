using CodeFlow.Start.Lib.WebTransfer.Base.Response;
using ExpensesControl.Application.UseCases.Base.Expense;

namespace ExpensesControl.Application.UseCases.Expenses.GetByUser.Dto.Response;

public class GetExpensesByUserResponse : BaseResponse, IResultResponse<IEnumerable<ExpenseResponse>>
{
	/// <summary>
	/// The response value of the use case.
	/// </summary>
	public IEnumerable<ExpenseResponse>? Result { get; set; } = default;

	/// <summary>
	/// Sets the result of the use case.
	/// </summary>
	/// <param name="result">The result returned by the use case.</param>
	public void SetResult(IEnumerable<ExpenseResponse> result)
	{
		Result = result;
	}
}

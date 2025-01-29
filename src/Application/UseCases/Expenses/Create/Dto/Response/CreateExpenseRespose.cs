using CodeFlow.Start.Package.WebTransfer.Base.Response;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto.Response;

/// <summary>
/// Response after creating an expense.
/// </summary>
public class CreateExpenseResponse : BaseResponse, IResultResponse<CreateResponse>
{
	/// <summary>
	/// The response value of the use case.
	/// </summary>
	public CreateResponse? Result { get; set; } = default;

	/// <summary>
	/// Sets the result of the use case.
	/// </summary>
	/// <param name="result">The result returned by the use case.</param>
	public void SetResult(CreateResponse result)
	{
		Result = result;
	}
}

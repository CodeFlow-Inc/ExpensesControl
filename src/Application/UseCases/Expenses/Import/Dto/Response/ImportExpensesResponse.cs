using CodeFlow.Start.Package.WebTransfer.Base.Response;
using ExpensesControl.Application.UseCases.Base.Response;

namespace ExpensesControl.Application.UseCases.Expenses.Import.Dto.Response;
public class ImportExpensesResponse : BaseResponse, IResultResponse<ImportResponse>
{
	/// <summary>
	/// The response value of the use case.
	/// </summary>
	public ImportResponse? Result { get; set; } = default;

	/// <summary>
	/// Sets the result of the use case.
	/// </summary>
	/// <param name="result">The result returned by the use case.</param>
	public void SetResult(ImportResponse result)
	{
		Result = result;
	}
}

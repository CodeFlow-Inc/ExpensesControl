using ExpensesControl.Application.UseCases.Base;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Response;

namespace ExpensesControl.Application.UseCases.Revenue.Dto.Response

/// <summary>
/// Response after creating an revenue.
/// </summary>
{
    public class CreateRevenueResponse : BaseResponse, IResultResponse<CreateResponse>
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
}
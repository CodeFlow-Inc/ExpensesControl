using ExpensesControl.Application.UseCases.Base;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;
using ExpensesControl.Application.UseCases.Expenses.GetByUser.Dto.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpensesControl.WebApi.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Operations related to expense management")]
public class ExpenseController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new expense.
    /// </summary>
    /// <param name="request">The expense creation request.</param>
    /// <returns>Returns the response of the expense creation.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new expense",
        Description = "Creates a new expense with the provided details.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Expense created successfully", typeof(IResultResponse<CreateResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data", typeof(BaseResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal error", typeof(BaseResponse))]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request)
    {
        var response = await mediator.Send(request);

        if (!response.IsSuccess && response.ErrorType == ErrorType.BusinessRuleError)
            return BadRequest(response);
        if (response.IsSuccess) return Ok(response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    /// <summary>
    /// Gets expenses by user.
    /// </summary>
    /// <param name="userCode">The user code to search expenses for.</param>
    /// <returns>Returns the list of expenses for the given user code.</returns>
    [HttpGet("/user/{userCode}")]
    [SwaggerOperation(
        Summary = "Get expenses by user code",
        Description = "Fetches expenses for the given user code.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Expenses retrieved successfully", typeof(GetExpensesByUserCodeRequest))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No expenses found for the given user code", null)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user code", typeof(BaseResponse))]
    public async Task<IActionResult> GetExpensesByUser(int userCode)
    {
        var request = new GetExpensesByUserCodeRequest(default) { UserCode = userCode };

        var response = await mediator.Send(request);

        if (!response.IsSuccess && response.ErrorType == ErrorType.BusinessRuleError)
            return BadRequest(response);
        if (response.IsSuccess && (response.Result == null || !response.Result.Any()))
            return NoContent();
        if (response.IsSuccess && response.Result != null && response.Result.Any())
            return Ok(response);
        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }
}

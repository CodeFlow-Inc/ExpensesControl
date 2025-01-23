using ExpensesControl.Application.UseCases.Base;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;
using ExpensesControl.Application.UseCases.Expenses.Create.Dto.Response;
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
    [SwaggerResponse(StatusCodes.Status200OK, "Expense created successfully", typeof(CreateExpenseResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data", typeof(BaseResponse))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal error", typeof(BaseResponse))]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request)
    {
        var response = await mediator.Send(request);

        if (response.IsValid) return Ok(response);

        if (!response.IsValid && response.ErrorType == ErrorType.BusinessRuleError)
            return BadRequest(response);
        else
            return StatusCode(StatusCodes.Status500InternalServerError, response);
    }
}

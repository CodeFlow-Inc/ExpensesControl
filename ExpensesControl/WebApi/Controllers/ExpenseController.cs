using ExpensesControl.Application.UseCases.Expenses.Create.Dto;
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
    /// <param name="input">The expense creation input.</param>
    /// <returns>Returns the output of the expense creation.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new expense", Description = "Creates a new expense with the provided details.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Expense created successfully", typeof(CreateExpenseOutput))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data", typeof(IEnumerable<string>))]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseInput input)
    {
        var output = await mediator.Send(input);

        return output.IsValid ? Ok(output) : BadRequest(output.ErrorMessages);
    }
}

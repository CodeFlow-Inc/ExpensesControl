using ExpensesControl.Application.UseCases.Expenses.Create.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesControl.WebApi.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ExpenseController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new expense.
    /// </summary>
    /// <param name="input">The expense creation input.</param>
    /// <returns>Returns the output of the expense creation.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseInput input)
    {
        var output = await mediator.Send(input);

        return output.IsValid ? Ok(output) : BadRequest(output.ErrorMessages);
    }
}

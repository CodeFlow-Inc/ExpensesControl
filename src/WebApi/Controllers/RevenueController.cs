using CodeFlow.Start.Lib.WebTransfer.Base;
using CodeFlow.Start.Lib.WebTransfer.Base.Response;
using ExpensesControl.Application.UseCases.Revenue.Create.Dto.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpensesControl.WebApi.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Operations related to revenue management")]
public class RevenueController(IMediator mediator) : ControllerBase
{
	/// <summary>
	/// Creates a new revenue.
	/// </summary>
	/// <param name="request">The expense creation revenue.</param>
	/// <returns>Returns the response of the revenue creation.</returns>
	[HttpPost]
	[SwaggerOperation(
		Summary = "Create a new revenue",
		Description = "Creates a new revenue with the provided details.")]
	[SwaggerResponse(StatusCodes.Status200OK, "Revenue created successfully", typeof(IResultResponse<CreateResponse>))]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data", typeof(BaseResponse))]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal error", typeof(BaseResponse))]
	public async Task<IActionResult> CreateRevenue([FromBody] CreateRevenueRequest request)
	{
		var response = await mediator.Send(request);

		if (response.IsSuccess) return Ok(response);

		if (!response.IsSuccess && response.ErrorType == ErrorType.BusinessRuleError)
			return BadRequest(response);
		else
			return StatusCode(StatusCodes.Status500InternalServerError, response);
	}
}

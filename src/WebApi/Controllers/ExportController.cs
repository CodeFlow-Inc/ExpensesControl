using CodeFlow.Start.Package.WebTransfer.Base;
using CodeFlow.Start.Package.WebTransfer.Base.Response;
using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ExpensesControl.WebApi.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Operations related to financial reports export")]
public class ExportController(IMediator mediator) : ControllerBase
{
	/// <summary>
	/// Exports monthly financial report in Excel format
	/// </summary>
	/// <param name="userCode">User identifier code</param>
	/// <param name="month">Report month (1-12)</param>
	/// <param name="year">Report year</param>
	/// <returns>Excel file with monthly financial data</returns>
	[HttpGet("monthly-report")]
	[SwaggerOperation(
		Summary = "Export monthly financial report",
		Description = "Generates Excel report with expenses and revenues for specified month")]
	[SwaggerResponse(StatusCodes.Status200OK, "Report generated successfully", typeof(FileResult))]
	[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid parameters", typeof(BaseResponse))]
	[SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal error", typeof(BaseResponse))]
	[ResponseCache(Duration = 3600)]
	public async Task<IActionResult> ExportMonthlyReport(
		[FromHeader, Required] int userCode,
		[FromQuery, Range(1, 12)] int month,
		[FromQuery, Range(2020, 2050)] int year)
	{
		var request = new ExportMonthlyReportRequest(userCode, month, year);
		var response = await mediator.Send(request);

		if (!response.IsSuccess)
		{
			return response.ErrorType switch
			{
				ErrorType.BusinessRuleError => BadRequest(response),
				_ => StatusCode(StatusCodes.Status500InternalServerError, response)
			};
		}

		return File(
			response.FileContent!,
			response.ContentType!,
			response.FileName);
	}
}
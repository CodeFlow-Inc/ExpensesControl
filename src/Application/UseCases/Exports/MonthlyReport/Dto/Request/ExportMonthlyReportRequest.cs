using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Request;
// ExportMonthlyReportRequest.cs
public record ExportMonthlyReportRequest(
	[Required] int UserCode,
	[Range(1, 12)] int Month,
	[Range(2000, 2100)] int Year
) : IRequest<ExportMonthlyReportResponse>;
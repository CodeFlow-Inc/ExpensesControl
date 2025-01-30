using ClosedXML.Excel;
using CodeFlow.Start.Package.Extensions;
using ExpensesControl.Application.ExcelReports;
using ExpensesControl.Application.Specs;
using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Request;
using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Response;
using ExpensesControl.Domain.Entities.AggregateRoot;
using ExpensesControl.Infrastructure.SqlServer.Repositories.Interface;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpensesControl.Application.UseCases.Exports.MonthlyReport;

public class ExportMonthlyReportUseCase(
	IUnitOfWork unitOfWork,
	IValidator<ExportMonthlyReportRequest> validator,
	ILogger<ExportMonthlyReportUseCase> logger) : IRequestHandler<ExportMonthlyReportRequest, ExportMonthlyReportResponse>
{
	public async Task<ExportMonthlyReportResponse> Handle(ExportMonthlyReportRequest request, CancellationToken cancellationToken)
	{
		var response = new ExportMonthlyReportResponse();

		if (!await validator.ValidateAndAddErrorsAsync(request, logger, response, cancellationToken))
			return response;

		var expenses = await unitOfWork.ExpenseRepository.ListBySpecificationAsync(
			new ExpenseSpec(request.UserCode, request.Month, request.Year), cancellationToken);

		var revenues = await unitOfWork.RevenueRepository.ListBySpecificationAsync(
			new RevenueSpec(request.UserCode, request.Month, request.Year), cancellationToken);

		var generator = new ExcelReportGenerator();

		AddExpensesWorksheet(generator, expenses);
		AddRevenuesWorksheet(generator, revenues);
		AddBalanceWorksheet(generator, expenses, revenues);

		response.SetResult(
			generator.GetFileBytes(),
			$"relatorio_{request.Month:00}_{request.Year}.xlsx",
			"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

		logger.LogInformation("Relatório gerado para o usuário {UserCode}", request.UserCode);
		return response;
	}

	private static void AddExpensesWorksheet(ExcelReportGenerator generator, IEnumerable<Expense> expenses)
	{
		var worksheet = generator.AddWorksheet("Despesas");
		ExcelReportGenerator.AddHeaders(worksheet, "Descrição", "Data", "Categoria", "Valor Total", "Tipo de Pagamento", "Parcelado");

		ExcelReportGenerator.AddDataRows(worksheet, expenses, (row, expense) =>
		{
			row.Cell(1).Value = expense.Description;
			row.Cell(2).Value = expense.StartDate.ToString("d", generator.Culture);
			row.Cell(3).Value = expense.Category.GetDescription();
			row.Cell(4).Value = expense.Payment.TotalValue;
			row.Cell(5).Value = expense.Payment.Type.GetDescription();
			row.Cell(6).Value = expense.Payment.IsInstallment ? "Sim" : "Não";

			ExcelReportGenerator.ApplyCurrencyFormat(row.Cell(4));
		});

		ExcelReportGenerator.ApplyConditionalFormatting(
			worksheet,
			column: 4,
			condition: cell => cell.GetValue<decimal>() > 1000,
			backgroundColor: XLColor.LightSalmon,
			bold: true
		);
	}

	private static void AddRevenuesWorksheet(ExcelReportGenerator generator, IEnumerable<Revenue> revenues)
	{
		var worksheet = generator.AddWorksheet("Receitas");
		ExcelReportGenerator.AddHeaders(worksheet, "Descrição", "Data Recebimento", "Tipo", "Valor", "Recorrente");

		ExcelReportGenerator.AddDataRows(worksheet, revenues, (row, revenue) =>
		{
			row.Cell(1).Value = revenue.Description;
			row.Cell(2).Value = revenue.ReceiptDate.ToString("d", generator.Culture);
			row.Cell(3).Value = revenue.Type.GetDescription();
			row.Cell(4).Value = revenue.Amount;
			row.Cell(5).Value = revenue.Recurrence.IsRecurring ? "Sim" : "Não";

			ExcelReportGenerator.ApplyCurrencyFormat(row.Cell(4));
		});

		ExcelReportGenerator.ApplyConditionalFormatting(
			worksheet,
			column: 4,
			condition: cell => cell.GetValue<decimal>() > 5000,
			backgroundColor: XLColor.LightGreen,
			bold: true
		);
	}

	private static void AddBalanceWorksheet(ExcelReportGenerator generator, IEnumerable<Expense> expenses, IEnumerable<Revenue> revenues)
	{
		var worksheet = generator.AddWorksheet("Saldo");
		ExcelReportGenerator.AddHeaders(worksheet, "Resumo Financeiro Mensal", "Valor");
		worksheet.Column(1).Width = 30;

		var totalReceitas = revenues.Sum(r => r.Amount);
		var totalDespesas = expenses.Sum(e => e.Payment.TotalValue);
		var saldoFinal = totalReceitas - totalDespesas;

		var data = new List<dynamic>
		{
			new { Descricao = "Total de Receitas", Valor = totalReceitas },
			new { Descricao = "Total de Despesas", Valor = totalDespesas },
			new { Descricao = "Saldo Final", Valor = saldoFinal }
		};

		ExcelReportGenerator.AddDataRows(worksheet, data, (row, item) =>
		{
			row.Cell(1).Value = item.Descricao;
			row.Cell(2).Value = item.Valor;
			ExcelReportGenerator.ApplyCurrencyFormat(row.Cell(2));

			if (item.Descricao == "Saldo Final")
			{
				var color = item.Valor >= 0 ? XLColor.Green : XLColor.Red;
				row.Cell(2).Style.Font.FontColor = color;
				row.Cell(2).Style.Font.Bold = true;
				ExcelReportGenerator.ApplyZebraStriping(worksheet, row.RowNumber(), headersCount: 2);
				worksheet.Range(row.RowNumber(), 1, row.RowNumber(), 2).Style
					.Fill.BackgroundColor = XLColor.LightYellow;
			}
		}, applyZebraStriping: false);
	}
}
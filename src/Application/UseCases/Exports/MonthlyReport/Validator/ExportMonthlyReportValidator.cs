using ExpensesControl.Application.UseCases.Exports.MonthlyReport.Dto.Request;
using FluentValidation;

namespace ExpensesControl.Application.UseCases.Exports.MonthlyReport.Validator;

/// <summary>
/// Validator for the ExportMonthlyReportRequest
/// </summary>
public class ExportMonthlyReportValidator : AbstractValidator<ExportMonthlyReportRequest>
{
	/// <summary>
	/// Constructor
	/// </summary>
	public ExportMonthlyReportValidator()
	{
		RuleFor(x => x.UserCode)
			.GreaterThan(0).WithMessage("Código de usuário inválido");

		RuleFor(x => x.Month)
			.InclusiveBetween(1, 12).WithMessage("Mês deve estar entre 1 e 12");

		RuleFor(x => x.Year)
			.InclusiveBetween(2000, 2100).WithMessage("Ano fora do intervalo válido");
	}
}

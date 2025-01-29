using CodeFlow.Csv.Package.Converter;
using CsvHelper.Configuration;
using ExpensesControl.Application.UseCases.Base.Records.Expense;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Import.Map;

/// <summary>
/// Maps the properties of the ExpenseRecord.
/// </summary>
public class ExpenseRecordMap : ClassMap<ExpenseRecord>
{
	/// <summary>
	/// Maps the properties of the ExpenseRecord.
	/// </summary>
	public ExpenseRecordMap()
	{
		Map(m => m.StartDate).Name("Data Inicio");

		Map(m => m.EndDate).Name("Data Fim");

		Map(m => m.Category).Name("Categoria")
			.TypeConverter<EnumDescriptionConverter<ExpenseCategory>>();

		Map(m => m.Description).Name("Descricao");

		Map(m => m.Payment.Type).Name("Tipo Pagamento")
			.TypeConverter<EnumDescriptionConverter<PaymentType>>();

		Map(m => m.Payment.IsInstallment).Name("Parcelado")
			.TypeConverterOption.BooleanValues(true, true, ["Yes", "Sim"])
			.TypeConverterOption.BooleanValues(false, true, ["No", "Nao", "Não"]);

		Map(m => m.Payment.InstallmentCount).Name("Qtd Parcelas");

		Map(m => m.Payment.TotalValue).Name("Valor Total");

		Map(m => m.Recurrence.IsRecurring).Name("Recorrente")
			.TypeConverterOption.BooleanValues(true, true, ["Yes", "Sim"])
			.TypeConverterOption.BooleanValues(false, true, ["No", "Nao", "Não"]);

		Map(m => m.Recurrence.MaxOccurrences).Name("Qtd Ocorrencias");

		Map(m => m.Recurrence.Periodicity).Name("Periodicidade")
			.TypeConverter<EnumDescriptionConverter<RecurrencePeriodicity>>();
	}
}

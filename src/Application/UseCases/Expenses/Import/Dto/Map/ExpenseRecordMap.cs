namespace ExpensesControl.Application.UseCases.Expenses.Import.Dto.Map;

using CodeFlow.Start.Lib.Helper.Csv;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ExpensesControl.Application.UseCases.Base.Records.Expense;
using ExpensesControl.Domain.Enums;

public class ExpenseRecordMap : ClassMap<ExpenseRecord>
{
	public ExpenseRecordMap()
	{
		Map(m => m.StartDate).Name("Data Inicio");

		Map(m => m.EndDate).Name("Data Fim");

		Map(m => m.Category).Name("Categoria")
			.TypeConverter<EnumDescriptionConverterr<ExpenseCategory>>();

		Map(m => m.Description).Name("Descricao");

		Map(m => m.Payment.Type).Name("Tipo Pagamento")
			.TypeConverter<EnumDescriptionConverterr<PaymentType>>();

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
			.TypeConverter<EnumDescriptionConverterr<RecurrencePeriodicity>>();
	}
}

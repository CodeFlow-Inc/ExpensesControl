namespace ExpensesControl.Application.UseCases.Expenses.Import.Dto.Map;
using CsvHelper.Configuration;
using ExpensesControl.Application.UseCases.Base.Records.Expense;

public class ExpenseRecordMap : ClassMap<ExpenseRecord>
{
	public ExpenseRecordMap()
	{
		Map(m => m.StartDate).Name("Data Inicio");
		Map(m => m.EndDate).Name("Data Fim");
		Map(m => m.Category).Name("Categoria");
		Map(m => m.Description).Name("Descricao");
		Map(m => m.Payment.Type).Name("Tipo Pagamento");
		Map(m => m.Payment.IsInstallment).Name("Parcelado");
		Map(m => m.Payment.InstallmentCount).Name("Qtd Parcelas");
		Map(m => m.Payment.TotalValue).Name("Valor Total");
		Map(m => m.Recurrence.IsRecurring).Name("Recorrente");
		Map(m => m.Recurrence.MaxOccurrences).Name("Qtd Ocorrencias");
		Map(m => m.Recurrence.Periodicity).Name("Periodicidade");
	}
}
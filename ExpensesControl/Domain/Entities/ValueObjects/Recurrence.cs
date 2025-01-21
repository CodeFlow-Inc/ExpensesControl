using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Domain.Entities.ValueObjects;

/// <summary>
/// Represents recurrence details for an expense.
/// </summary>
public class Recurrence
{
    /// <summary>
    /// Indicates whether the expense is recurring.
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Periodicity of the recurrence (e.g., daily, weekly, monthly, yearly).
    /// </summary>
    public RecurrencePeriodicity Periodicity { get; set; }

    /// <summary>
    /// Maximum number of occurrences for the recurrence.
    /// If null, the recurrence is indefinite.
    /// </summary>
    public int? MaxOccurrences { get; set; }

    /// <summary>
    /// Valida as regras relacionadas à recorrência de uma despesa, adicionando erros ao parâmetro de saída.
    /// </summary>
    /// <param name="errors">Uma lista de mensagens de erro retornadas caso alguma regra de validação seja violada.</param>
    /// <returns>
    /// Retorna <c>true</c> se a validação passar sem erros; caso contrário, <c>false</c>.
    /// </returns>
    public bool Validate(out List<string> errors)
    {
        errors = [];

        if (IsRecurring)
        {
            if (MaxOccurrences.HasValue && MaxOccurrences <= 0)
            {
                errors.Add("O número máximo de ocorrências deve ser maior que zero.");
            }
        }
        else
        {
            if (MaxOccurrences.HasValue)
            {
                errors.Add("Despesas não recorrentes não devem ter número máximo de ocorrências.");
            }
        }

        return errors.Count == 0;
    }

}

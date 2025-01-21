using Destructurama.Attributed;
using ExpensesControl.Domain.Entities.Base;
using ExpensesControl.Domain.Entities.ValueObjects;
using ExpensesControl.Domain.Enums;
using System.Collections.Generic;

namespace ExpensesControl.Domain.Entities.AggregateRoot;

/// <summary>
/// Represents an expense entry.
/// </summary>
public class Expense : BaseEntity<int>
{
    /// <summary>
    /// User code associated with the expense.
    /// </summary>
    [LogMasked]
    public int UserCode { get; set; }

    /// <summary>
    /// Description of the expense.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Value or amount of the expense.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Start date when the expense was incurred.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date for the expense (nullable).
    /// If the expense is recurring, this can be null to indicate an indefinite period.
    /// Otherwise, it is equal to StartDate.
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// Category of the expense.
    /// </summary>
    public ExpenseCategory Category { get; set; }

    /// <summary>
    /// Recurrence details for the expense.
    /// </summary>
    public Recurrence Recurrence { get; set; } = new Recurrence();

    /// <summary>
    /// Payment method used for the expense.
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; } = new PaymentMethod();

    /// <summary>
    /// Additional notes or details about the expense.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Valida a consistência dos dados de uma despesa, verificando regras de negócio específicas.
    /// </summary>
    /// <param name="errors">
    /// Uma lista de mensagens de erro retornadas caso alguma regra de validação seja violada.
    /// </param>
    /// <returns>
    /// Retorna <c>true</c> se a validação passar sem erros; caso contrário, <c>false</c>.
    /// </returns>
    public bool Validate(out List<string> errors)
    {
        errors = [];
        if (Value <= 0)
        {
            errors.Add("O valor deve ser maior que zero.");
        }

        if (Recurrence.IsRecurring)
        {
            if (EndDate.HasValue && EndDate < StartDate)
            {
                errors.Add("A data final não pode ser anterior à data inicial.");
            }
        }
        else
        {
            if (EndDate.HasValue && EndDate != StartDate)
            {
                errors.Add("Para despesas não recorrentes, a data final deve ser igual à data inicial.");
            }

            EndDate = StartDate;
        }

        if (UserCode <= 0)
        {
            errors.Add("O código do usuário deve ser um número inteiro positivo.");
        }

        if(!Recurrence.Validate(out var errorsRecurrence)) 
            errors.AddRange(errorsRecurrence);

        if (!PaymentMethod.Validate(Value, out var errorsPaymentMethod)) 
            errors.AddRange(errorsPaymentMethod);

        return errors.Count == 0;
    }
}

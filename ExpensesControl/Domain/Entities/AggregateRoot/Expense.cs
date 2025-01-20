using Destructurama.Attributed;
using ExpensesControl.Domain.Entities.Base;
using ExpensesControl.Domain.Entities.ValueObjects;
using ExpensesControl.Domain.Enums;

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
    /// Validates the consistency of the expense data.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when validation fails.</exception>
    public void Validate()
    {
        if (Value <= 0)
        {
            throw new InvalidOperationException("O valor deve ser maior que zero.");
        }

        if (Recurrence.IsRecurring)
        {
            if (EndDate.HasValue && EndDate < StartDate)
            {
                throw new InvalidOperationException("A data final não pode ser anterior à data inicial.");
            }
        }
        else
        {
            if (EndDate.HasValue && EndDate != StartDate)
            {
                throw new InvalidOperationException("Para despesas não recorrentes, a data final deve ser igual à data inicial.");
            }

            EndDate = StartDate;
        }

        if (UserCode <= 0)
        {
            throw new InvalidOperationException("O código do usuário deve ser um número inteiro positivo.");
        }
        Recurrence.Validate();
        PaymentMethod.Validate(Value);
    }
}

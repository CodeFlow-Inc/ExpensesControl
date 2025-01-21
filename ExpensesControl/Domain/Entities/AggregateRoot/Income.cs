using Destructurama.Attributed;
using ExpensesControl.Domain.Entities.Base;
using ExpensesControl.Domain.Entities.ValueObjects;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Domain.Entities.AggregateRoot
{
    /// <summary>
    /// Represents an Income entry.
    /// </summary>
    public class Income : BaseEntity<int>
    {
        /// <summary>
        /// User code associated with the expense.
        /// </summary>
        [LogMasked]
        public int UserCode { get; set; }

        /// <summary>
        /// Description of the Income.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Amount of the Income
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date of the Income
        /// </summary>
        public DateOnly ReceiptDate { get; set; }

        /// <summary>
        /// Type of the Income
        /// </summary>
        public TypeIncome Type { get; set; }

        public void Validate()
        {
            if (Amount <= 0)
            {
                throw new InvalidOperationException("O valor deve ser maior que zero.");
            }

            if (UserCode <= 0)
            {
                throw new InvalidOperationException("O código do usuário deve ser um número inteiro positivo.");
            }

            var recurrence = new Recurrence();
            recurrence.Validate();

            var paymentMethod = new PaymentMethod();
            paymentMethod.Validate(Amount);
        }
    }
}

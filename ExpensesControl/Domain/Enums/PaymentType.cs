using System.ComponentModel;

namespace ExpensesControl.Domain.Enums;

/// <summary>
/// Enum representing types of payment methods.
/// </summary>
public enum PaymentType
{
    /// <summary>
    /// Payment made using cash.
    /// </summary>
    [Description("Dinheiro")]
    Cash,

    /// <summary>
    /// Payment made using a credit card.
    /// </summary>
    [Description("Cartão de Crédito")]
    CreditCard,

    /// <summary>
    /// Payment made using a debit card.
    /// </summary>
    [Description("Cartão de Débito")]
    DebitCard,

    /// <summary>
    /// Payment made via bank transfer.
    /// </summary>
    [Description("Transferência Bancária")]
    BankTransfer,

    /// <summary>
    /// Payment made using the Pix system (instant payment system in Brazil).
    /// </summary>
    [Description("Pix")]
    Pix,

    /// <summary>
    /// Any other type of payment not listed above.
    /// </summary>
    [Description("Outro")]
    Other
}

﻿using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Expenses.Create.Dto.Request;

public class CreateExpensePaymentRequest
{
    public PaymentType Type { get; set; }
    public bool IsInstallment { get; set; }
    public int? InstallmentCount { get; set; }
    public decimal TotalValue { get; set; }
    public string? Notes { get; set; }
}

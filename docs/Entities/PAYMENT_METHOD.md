## **Class: PaymentMethod**

### **Description**
Represents a payment method for an expense.

### **Properties**

| Property           | Type         | Description                                                                 |
|--------------------|--------------|-----------------------------------------------------------------------------|
| `Type`             | `PaymentType` | Type of the payment (e.g., cash, credit card).                              |
| `IsInstallment`    | `bool`       | Indicates whether the payment is in installments.                           |
| `InstallmentCount` | `int?`       | Number of installments, applicable if the payment is in installments.       |
| `InstallmentValue` | `decimal?`   | Value of each installment, applicable if the payment is in installments.    |
| `Notes`            | `string?`    | Additional details about the payment method.                                |

### **Methods**

#### `void Validate(decimal totalValue)`
Validates the consistency of the payment method data.

- **Throws**: `InvalidOperationException` if:
  - The number of installments or value of each installment is invalid for installment payments.
  - The total value of installments does not match the total payment amount.
  - Non-installment payments have installment-specific fields.

---

---

## **Enum: PaymentType**

### **Description**
Enumeration representing types of payment methods.

### **Values**

| Name              | Description                                    |
|-------------------|------------------------------------------------|
| `Cash`            | Payment made using cash.                      |
| `CreditCard`      | Payment made using a credit card.             |
| `DebitCard`       | Payment made using a debit card.              |
| `BankTransfer`    | Payment made via bank transfer.               |
| `Pix`             | Payment made using the Pix system.            |
| `Other`           | Any other type of payment not listed above.   |

## **Class: Expense**

### **Description**
Represents an expense entry.

### **Properties**

| Property         | Type                | Description                                                                                       |
|------------------|---------------------|---------------------------------------------------------------------------------------------------|
| `UserCode`       | `int`              | User code associated with the expense.                                                           |
| `Description`    | `string?`          | Description of the expense.                                                                      |
| `Value`          | `decimal`          | Value or amount of the expense. Must be greater than zero.                                       |
| `StartDate`      | `DateOnly`         | Start date when the expense was incurred.                                                        |
| `EndDate`        | `DateOnly?`        | End date for the expense. If recurring, it can be null for an indefinite period. Otherwise, equals `StartDate`. |
| `Category`       | `ExpenseCategory`  | Category of the expense.                                                                         |
| `Recurrence`     | `Recurring`        | Recurrence details for the expense.                                                              |
| `PaymentMethod`  | `PaymentMethod`    | Payment method used for the expense.                                                             |
| `Notes`          | `string?`          | Additional notes or details about the expense.                                                   |
| `Status`         | `Status`           | Status of the expense (e.g., Paid, Overdue, etc.).                                               |

### **Methods**

#### `void Validate()`
Validates the consistency of the expense data.

- **Throws**: `InvalidOperationException` if:
  - `Value` is less than or equal to zero.
  - `EndDate` is earlier than `StartDate` for recurring expenses.
  - `EndDate` is not equal to `StartDate` for non-recurring expenses.
  - `UserCode` is not a positive integer.

---

## **Enum: ExpenseCategory**

### **Description**
Enumeration representing different expense categories.

### **Values**

| Name                   | Value | Description                             |
|------------------------|-------|-----------------------------------------|
| `Food`                 | 10    | Food-related expenses.                  |
| `Health`               | 20    | Health and medical expenses.            |
| `Housing`              | 30    | Housing-related expenses.               |
| `Transportation`       | 40    | Transportation-related expenses.        |
| `Education`            | 50    | Educational expenses.                   |
| `Pets`                 | 60    | Expenses related to pets.               |
| `Clothing`             | 70    | Clothing-related expenses.              |
| `Leisure`              | 80    | Leisure and entertainment expenses.     |
| `Technology`           | 90    | Expenses related to technology.         |
| `GiftsAndCelebrations` | 100   | Expenses related to gifts and celebrations. |
| `Other`                | 110   | Miscellaneous expenses.                 |

---

## **Enum: Status**

### **Description**
Represents the status of an expense.

### **Values**

| Name         | Value | Description                          |
|--------------|-------|--------------------------------------|
| `Overdue`    | 10    | The expense is overdue.              |
| `ToExpire`   | 20    | The expense is due soon.             |
| `ToPay`      | 30    | The expense is to be paid.           |
| `Paid`       | 40    | The expense has been paid.           |
| `Cancelled`  | 50    | The expense has been cancelled.      |

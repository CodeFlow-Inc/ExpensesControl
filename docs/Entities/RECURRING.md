## **Class: Recurring**

### **Description**
Represents recurrence details for an expense.

### **Properties**

| Property        | Type                    | Description                                            |
|-----------------|-------------------------|--------------------------------------------------------|
| `IsRecurring`   | `bool`                 | Indicates whether the expense is recurring.            |
| `Periodicity`   | `RecurrencePeriodicity` | Periodicity of the recurrence (e.g., daily, weekly).   |
| `MaxOccurrences`| `int?`                 | Maximum number of occurrences for the recurrence.      |

### **Methods**

#### `void Validate()`
Validates the consistency of the recurrence data.

- **Throws**: `InvalidOperationException` if:
  - The maximum number of occurrences is invalid for recurring expenses.
  - Non-recurring expenses have a maximum number of occurrences.

---

## **Enum: RecurrencePeriodicity**

### **Description**
Enumeration representing recurrence periodicity.

### **Values**

| Name        | Description      |
|-------------|------------------|
| `Daily`     | Recurs daily.    |
| `Weekly`    | Recurs weekly.   |
| `Monthly`   | Recurs monthly.  |
| `Yearly`    | Recurs yearly.   |
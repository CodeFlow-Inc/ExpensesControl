### **Project: Personal Expense Management API**

#### **Description**
A simple and efficient REST API to manage personal expenses. The focus is on implementing small but well-optimized features, such as expense categorization, summary reports, and basic authentication. It aims to highlight best practices, including the use of design patterns, testing, and query optimization.

---

### **Features**

1. **Expense Management**
   - CRUD operations for expenses:
     - Fields: description, amount, date, category (e.g., food, transportation, etc.).
   - Input validations (e.g., amount must be greater than zero).

2. **Simple Reports**
   - Total expenses for specific periods (monthly, weekly...).
   - Total expenses by category.

3. **Export**
   - Endpoint to export expenses in CSV format.

---

### **Endpoint Structure**

- **Expenses**
  - `GET /api/expenses` - Retrieve the user's expenses.
  - `POST /api/expenses` - Add a new expense.
  - `PUT /api/expenses/{id}` - Update an expense.
  - `DELETE /api/expenses/{id}` - Delete an expense.

- **Reports**
  - `GET /api/reports/summary` - Summary of expenses by period.
  - `GET /api/reports/by-category` - Total expenses by category.

- **Export**
  - `GET /api/expenses/export` - Export expenses in CSV format.
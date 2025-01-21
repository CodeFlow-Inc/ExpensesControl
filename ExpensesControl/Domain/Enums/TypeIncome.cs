using System.ComponentModel;

namespace ExpensesControl.Domain.Enums
{
    /// <summary>
    /// Represents the type of income.
    /// </summary>
    public enum TypeIncome
    {
        /// <summary>
        /// Salary.
        /// </summary>
        [Description("Salário")]
        Salary = 10,

        /// <summary>
        /// Food allowance (Vale Refeição).
        /// </summary>
        [Description("Vale Refeição")]
        FoodAllowance = 20,

        /// <summary>
        /// Grocery allowance (Vale Alimentação).
        /// </summary>
        [Description("Vale Alimentação")]
        GroceryAllowance = 30,

        /// <summary>
        /// Bonus.
        /// </summary>
        [Description("Bônus")]
        Bonus = 40,

        /// <summary>
        /// Other types of income.
        /// </summary>
        [Description("Outros")]
        Others = 50
    }
}

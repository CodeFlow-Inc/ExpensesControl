using System.ComponentModel;

namespace ExpensesControl.Domain.Enums;

/// <summary>
/// Enumeration representing different expense categories.
/// </summary>
public enum ExpenseCategory
{
    /// <summary>
    /// Food-related expenses.
    /// </summary>
    [Description("Alimentação")]
    Food = 10,

    /// <summary>
    /// Health and medical expenses.
    /// </summary>
    [Description("Saúde")]
    Health = 20,

    /// <summary>
    /// Housing-related expenses.
    /// </summary>
    [Description("Casa")]
    Housing = 30,

    /// <summary>
    /// Transportation-related expenses.
    /// </summary>
    [Description("Transporte")]
    Transportation = 40,

    /// <summary>
    /// Educational expenses, such as school or courses.
    /// </summary>
    [Description("Educação")]
    Education = 50,

    /// <summary>
    /// Expenses related to pets, such as food or veterinary services.
    /// </summary>
    [Description("Animais de Estimação")]
    Pets = 60,

    /// <summary>
    /// Clothing-related expenses.
    /// </summary>
    [Description("Roupa")]
    Clothing = 70,

    /// <summary>
    /// Leisure and entertainment expenses.
    /// </summary>
    [Description("Lazer")]
    Leisure = 80,

    /// <summary>
    /// Expenses related to technology, such as gadgets or software.
    /// </summary>
    [Description("Tecnologia")]
    Technology = 90,

    /// <summary>
    /// Expenses related to gifts and celebrations.
    /// </summary>
    [Description("Presentes e Celebrações")]
    GiftsAndCelebrations = 100,

    /// <summary>
    /// Miscellaneous or other types of expenses.
    /// </summary>
    [Description("Outros")]
    Other = 110
}

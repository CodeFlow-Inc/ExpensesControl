using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace ExpensesControl.Application.ExcelReports;

/// <summary>
/// Provides extension methods for generating Excel reports with various formatting and styling options.
/// </summary>
public static class ExcelReportGeneratorExtensions
{
	private const string CurrencyFormat = "R$ #,##0.00;[Red]-R$ #,##0.00";

	/// <summary>
	/// Adds headers to the worksheet.
	/// </summary>
	/// <param name="worksheet">The Excel worksheet to which headers will be added.</param>
	/// <param name="headers">The headers to add to the worksheet.</param>
	public static void AddHeaders(this ExcelWorksheet worksheet, params string[] headers)
	{
		for (int i = 0; i < headers.Length; i++)
		{
			var cell = worksheet.Cells[1, i + 1];
			cell.Value = headers[i];
			cell.Style.Font.Bold = true;
			cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
			cell.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
			cell.Style.Font.Color.SetColor(Color.White);
			cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);
		}
	}

	/// <summary>
	/// Adds data rows to the worksheet.
	/// </summary>
	/// <typeparam name="T">The type of the data to populate the rows with.</typeparam>
	/// <param name="worksheet">The Excel worksheet to which data rows will be added.</param>
	/// <param name="data">The data to be added to the worksheet.</param>
	/// <param name="populateRowAction">An action to populate each row with data.</param>
	/// <param name="startRow">The row index to start adding data (defaults to 2).</param>
	/// <param name="applyZebraStriping">Indicates whether to apply zebra striping to rows (defaults to true).</param>
	public static void AddDataRows<T>(
		this ExcelWorksheet worksheet,
		IEnumerable<T> data,
		Action<int, T> populateRowAction,
		int startRow = 2,
		bool applyZebraStriping = true)
	{
		var row = startRow;
		foreach (var item in data)
		{
			populateRowAction(row, item);

			if (applyZebraStriping)
			{
				ApplyZebraStriping(worksheet, row, headersCount: worksheet.Dimension.End.Column);
			}

			row++;
		}

		if (row > startRow)
		{
			var dataRange = worksheet.Cells[startRow, 1, row - 1, worksheet.Dimension.End.Column];
			dataRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
			dataRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
			worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
		}
	}

	/// <summary>
	/// Applies zebra striping to the specified row.
	/// </summary>
	/// <param name="worksheet">The Excel worksheet where zebra striping will be applied.</param>
	/// <param name="row">The row index to apply zebra striping to.</param>
	/// <param name="headersCount">The number of columns in the worksheet to apply the striping to.</param>
	public static void ApplyZebraStriping(this ExcelWorksheet worksheet, int row, int headersCount)
	{
		var dataRowRange = worksheet.Cells[row, 1, row, headersCount];
		dataRowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
		dataRowRange.Style.Fill.BackgroundColor.SetColor(row % 2 == 0 ? Color.White : Color.LightGray);
	}

	/// <summary>
	/// Applies currency format to the specified cell.
	/// </summary>
	/// <param name="cell">The cell to which the currency format will be applied.</param>
	public static void ApplyCurrencyFormat(this ExcelRange cell)
	{
		cell.Style.Numberformat.Format = CurrencyFormat;
	}

	/// <summary>
	/// Applies conditional formatting to cells in a specific column based on a condition.
	/// </summary>
	/// <param name="worksheet">The Excel worksheet where conditional formatting will be applied.</param>
	/// <param name="column">The column index to apply conditional formatting to.</param>
	/// <param name="condition">A condition that determines whether a cell should be formatted.</param>
	/// <param name="backgroundColor">The background color to apply when the condition is met.</param>
	/// <param name="bold">Indicates whether the cell text should be bold (defaults to false).</param>
	public static void ApplyConditionalFormatting(
				this ExcelWorksheet worksheet,
				int column,
				Func<ExcelRangeBase, bool> condition,
				Color backgroundColor,
				bool bold = false)
	{
		var dataCells = worksheet.Cells[2, column, worksheet.Dimension.End.Row, column]
			.Where(cell => cell.Value != null && (cell.Value is double || cell.Value is decimal))
			.ToList();

		foreach (var cell in dataCells.Where(condition))
		{
			cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
			cell.Style.Fill.BackgroundColor.SetColor(backgroundColor);
			if (bold) cell.Style.Font.Bold = true;
		}
	}
}

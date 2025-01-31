using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace ExpensesControl.Application.ExcelReports;
public static class ExcelReportGeneratorExtensions
{
	private const string CurrencyFormat = "R$ #,##0.00;[Red]-R$ #,##0.00";

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

	public static void ApplyZebraStriping(this ExcelWorksheet worksheet, int row, int headersCount)
	{
		var dataRowRange = worksheet.Cells[row, 1, row, headersCount];
		dataRowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
		dataRowRange.Style.Fill.BackgroundColor.SetColor(row % 2 == 0 ? Color.White : Color.LightGray);
	}

	public static void ApplyCurrencyFormat(this ExcelRange cell)
	{
		cell.Style.Numberformat.Format = CurrencyFormat;
	}

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

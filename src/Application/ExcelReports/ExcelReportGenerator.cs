using ClosedXML.Excel;
using System.Globalization;

namespace ExpensesControl.Application.ExcelReports;

public class ExcelReportGenerator(CultureInfo? culture = null)
{
	private const string CurrencyFormat = "R$ #,##0.00;[Red]-R$ #,##0.00";
	private readonly CultureInfo _culture = culture ?? CultureInfo.CurrentCulture;
	private readonly XLWorkbook _workbook = new();

	public CultureInfo Culture => _culture;
	public XLWorkbook Workbook => _workbook;

	public IXLWorksheet AddWorksheet(string sheetName)
	{
		return _workbook.Worksheets.Add(sheetName);
	}

	public static void AddHeaders(IXLWorksheet worksheet, params string[] headers)
	{
		var headerRange = worksheet.Range(1, 1, 1, headers.Length);

		headerRange.Style.Font.Bold = true;
		headerRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
		headerRange.Style.Font.FontColor = XLColor.White;
		headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
		headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

		for (int i = 0; i < headers.Length; i++)
		{
			worksheet.Cell(1, i + 1).Value = headers[i];
		}
	}

	public static void AddDataRows<T>(
		IXLWorksheet worksheet,
		IEnumerable<T> data,
		Action<IXLRow, T> populateRowAction,
		int startRow = 2,
		bool applyZebraStriping = true)
	{
		var row = startRow;
		foreach (var item in data)
		{
			var currentRow = worksheet.Row(row);
			populateRowAction(currentRow, item);

			if (applyZebraStriping)
			{
				ApplyZebraStriping(worksheet, row, headersCount: worksheet.ColumnsUsed().Count());
			}

			row++;
		}

		if (row > startRow)
		{
			var dataRange = worksheet.Range(startRow, 1, row - 1, worksheet.ColumnsUsed().Count());
			dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
			dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
			worksheet.Columns().AdjustToContents();
		}
	}

	public static void ApplyZebraStriping(IXLWorksheet worksheet, int row, int headersCount)
	{
		var dataRowRange = worksheet.Range(row, 1, row, headersCount);
		dataRowRange.Style.Fill.BackgroundColor = row % 2 == 0
			? XLColor.White
			: XLColor.LightGray;
	}

	public static void ApplyCurrencyFormat(IXLCell cell)
	{
		cell.Style.NumberFormat.Format = CurrencyFormat;
	}

	public static void ApplyConditionalFormatting(
		IXLWorksheet worksheet,
		int column,
		Func<IXLCell, bool> condition,
		XLColor backgroundColor,
		bool bold = false)
	{
		var dataCells = worksheet.Column(column).CellsUsed()
			.Where(cell => cell.Address.RowNumber > 1 && cell.DataType == XLDataType.Number);

		foreach (var cell in dataCells.Where(condition))
		{
			cell.Style.Fill.BackgroundColor = backgroundColor;
			if (bold) cell.Style.Font.Bold = true;
		}
	}

	public byte[] GetFileBytes()
	{
		using var stream = new MemoryStream();
		_workbook.SaveAs(stream);
		return stream.ToArray();
	}
}
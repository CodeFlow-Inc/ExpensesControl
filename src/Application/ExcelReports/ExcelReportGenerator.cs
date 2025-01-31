using OfficeOpenXml;

namespace ExpensesControl.Application.ExcelReports;

/// <summary>
/// Generates Excel reports using the EPPlus library.
/// </summary>
public class ExcelReportGenerator()
{
	private readonly ExcelPackage _workbook = new();

	/// <summary>
	/// Gets the Excel workbook instance.
	/// </summary>
	public ExcelPackage Workbook => _workbook;

	/// <summary>
	/// Adds a new worksheet to the workbook.
	/// </summary>
	/// <param name="sheetName">The name of the worksheet to be added.</param>
	/// <returns>The created <see cref="ExcelWorksheet"/> instance.</returns>
	public ExcelWorksheet AddWorksheet(string sheetName)
	{
		return _workbook.Workbook.Worksheets.Add(sheetName);
	}

	/// <summary>
	/// Converts the workbook into a byte array.
	/// </summary>
	/// <returns>A byte array representing the Excel file.</returns>
	public byte[] GetFileBytes()
	{
		return _workbook.GetAsByteArray();
	}
}

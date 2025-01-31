using OfficeOpenXml;

namespace ExpensesControl.Application.ExcelReports;

public class ExcelReportGenerator()
{
	private readonly ExcelPackage _workbook = new();
	public ExcelPackage Workbook => _workbook;

	public ExcelWorksheet AddWorksheet(string sheetName)
	{
		return _workbook.Workbook.Worksheets.Add(sheetName);
	}

	public byte[] GetFileBytes()
	{
		return _workbook.GetAsByteArray();
	}
}
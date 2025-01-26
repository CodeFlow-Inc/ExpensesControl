using CsvHelper;
using CsvHelper.Configuration;

namespace ExpensesControl.Application.UseCases.Expenses.Import.Dto.Validator;

/// <summary>
/// 
/// </summary>
/// <param name="classMap"></param>
public class CsvFileValidator(ClassMap classMap)
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="fileName"></param>
	/// <returns></returns>
	public static bool IsCsvFile(string fileName)
	{
		return fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="fileStream"></param>
	/// <param name="errors"></param>
	/// <returns></returns>
	public bool HasValidHeaders(Stream fileStream, CsvConfiguration csvConfiguration, out List<string> errors)
	{
		errors = [];

		using var reader = new StreamReader(fileStream);
		using var csv = new CsvReader(reader, csvConfiguration);
		csv.Context.RegisterClassMap(classMap);
		csv.Read();
		csv.ReadHeader();
		var headerRecord = csv.HeaderRecord;

		var expectedHeaders = classMap.MemberMaps.Select(m => m.Data.Names.FirstOrDefault()).ToArray();
		if (!expectedHeaders.All(header => headerRecord!.Contains(header)))
		{
			errors.Add("The CSV file headers are incorrect.");
			return false;
		}

		return true;
	}
}


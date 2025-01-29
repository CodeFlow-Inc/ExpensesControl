namespace ExpensesControl.Application.UseCases.Base.Response;

public class ImportResponse
{
	public int TotalRecords { get; set; }
	public int SuccessfulRecords { get; set; }
	public int FailedRecords { get; set; }
	public List<string> Errors { get; set; } = [];
}
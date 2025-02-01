using CodeFlow.Start.Package.WebTransfer.Base.Response;

namespace ExpensesControl.Application.UseCases.Base.Response;

/// <summary>
/// Base class for export responses.
/// </summary>
public abstract class ExportResponse : BaseResponse
{
	public byte[]? FileContent { get; private set; }
	public string? FileName { get; private set; }
	public string? ContentType { get; private set; }

	/// <summary>
	/// Sets the result of the export operation.
	/// </summary>
	/// <param name="content"></param>
	/// <param name="fileName"></param>
	/// <param name="contentType"></param>
	public void SetResult(byte[] content, string fileName, string contentType)
	{
		FileContent = content;
		FileName = fileName;
		ContentType = contentType;
	}

}

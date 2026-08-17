using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetFabric.Hyperlinq;
using Exception = System.Exception;

namespace DragonSpark.Azure.Storage.Uploads;

public abstract class UploadsControllerBase : ControllerBase
{
	readonly FileRequests                            _requests;
	readonly FormFiles                               _files;
	readonly ISelect<IFormCollection, UploadRequest> _request;
	readonly Error                                   _error;

	protected UploadsControllerBase(FileRequests requests, Error error)
		: this(requests, FormFiles.Default, UploadRequestParser.Default, error) {}

	// ReSharper disable once TooManyDependencies
	protected UploadsControllerBase(FileRequests requests, FormFiles files,
	                                ISelect<IFormCollection, UploadRequest> request, Error error)
	{
		_requests = requests;
		_files    = files;
		_request  = request;
		_error    = error;
	}

	protected async ValueTask<IActionResult> View(Guid identifier, string name, bool allowDownload = true)
	{
		var (_, _, view) = _requests;
		var entry  = await view.Off(new(new(User, new(identifier, name)), HttpContext.RequestAborted));
		var result = entry is not null ? await ViewEntry(entry, allowDownload).Off() : NotFound();
		return result;
	}

	protected virtual Task<IActionResult> ViewEntry(IStorageEntry entry, bool allowDownload)
	{
		RequestFileResultBase request = allowDownload ? RequestFileResult.Default : SourceFileRequest.Default;
		var                   result  = request.Allocate(new(this, entry));
		return result;
	}

	public virtual async ValueTask<IActionResult> Save(IList<IFormFile> files)
	{
		try
		{
			var (save, _, _)               = _requests;
			var (workspace, session, last) = _request.Get(Request.Form);
			var header = Request.Headers;
			foreach (var input in files.AsValueEnumerable())
			{
				var file = _files.Get(new(header, input));
				await save.Off(new(new(User, new(workspace, session, file)), HttpContext.RequestAborted));
			}

			return Ok(new { status = $"{(last ? "File" : "Chunk")} uploaded successfully" });
		}
		catch (Exception e)
		{
			return Handle(e, "uploading");
		}
	}

	IActionResult Handle(Exception e, string action)
	{
		_error.Execute(e, action);
		return StatusCode(500,
		                  new
		                  {
			                  status  = "Error",
			                  message = $"Problem with {action} file.  System administrators have been notified."
		                  });
	}

	public virtual async ValueTask<IActionResult> Remove(IList<IFormFile> files)
	{
		try
		{
			var (_, remove, _)          = _requests;
			var (workspace, session, _) = _request.Get(Request.Form);
			foreach (var file in files.AsValueEnumerable())
			{
				await remove.Off(new(new(User, new(workspace, session, file)), HttpContext.RequestAborted));
			}

			return Ok(new { status = "File removed successfully" });
		}
		catch (Exception e)
		{
			return Handle(e, "removing");
		}
	}

	public sealed class Error : LogErrorException<string>
	{
		public Error(ILogger<Error> logger) : base(logger, "An error occurred while {Action} files") {}
	}
}
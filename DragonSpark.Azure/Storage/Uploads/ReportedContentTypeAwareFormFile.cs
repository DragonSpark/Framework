using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class ReportedContentTypeAwareFormFile : IFormFile
{
	readonly IFormFile _previous;

	public ReportedContentTypeAwareFormFile(IFormFile previous, string contentType)
	{
		_previous   = previous;
		ContentType = contentType;
	}

	public Stream OpenReadStream() => _previous.OpenReadStream();

	public void CopyTo(Stream target)
	{
		_previous.CopyTo(target);
	}

	public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new())
		=> _previous.CopyToAsync(target, cancellationToken);

	public string ContentType { get; }

	public string ContentDisposition => _previous.ContentDisposition;

	public IHeaderDictionary Headers => _previous.Headers;

	public long Length => _previous.Length;

	public string Name => _previous.Name;

	public string FileName => _previous.FileName;
}
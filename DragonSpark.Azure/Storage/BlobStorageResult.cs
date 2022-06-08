using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

/// <summary>
/// ATTRIBUTION: https://medium.com/codex/efficiently-serving-blobs-from-azure-storage-in-asp-net-core-using-custom-actionresult-33399faaecbf
/// </summary>
public class BlobStorageResult : FileResult
{
	public BlobStorageResult(string blobUrl, string contentType) : base(contentType)
	{
		if (string.IsNullOrWhiteSpace(BlobUrl = blobUrl))
		{
			throw new ArgumentException($"'{nameof(blobUrl)}' cannot be null or whitespace.", nameof(blobUrl));
		}
	}

	/// <summary>Gets the URL for the block blob to be returned.</summary>
	public string BlobUrl { get; }

	/// <summary>Gets or sets the <c>Content-Type</c> header if the value is already known.</summary>
	public long? ContentLength { get; set; }

	/// <summary>
	/// Gets or sets whether blob properties should be retrieved before downloading.
	/// Useful when the file size is not known in advance
	/// </summary>
	public bool GetPropertiesBeforeDownload { get; set; }

	/// <inheritdoc/>
	public override Task ExecuteResultAsync(ActionContext context)
	{
		if (context == null) throw new ArgumentNullException(nameof(context));

		return context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<BlobStorageResult>>()
		              .ExecuteAsync(context, this);
	}
}
using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

/// <summary>
/// ATTRIBUTION: https://medium.com/codex/efficiently-serving-blobs-from-azure-storage-in-asp-net-core-using-custom-actionresult-33399faaecbf
/// </summary>
public class BlobStorageResultExecutor : FileResultExecutorBase, IActionResultExecutor<BlobStorageResult>
{
	readonly BlobClient _client;

	public BlobStorageResultExecutor(BlobClient client, ILogger<BlobStorageResultExecutor> logger) : base(logger)
		=> _client = client;

	public async Task ExecuteAsync(ActionContext context, BlobStorageResult result)
	{
		var cancellationToken = context.HttpContext.RequestAborted;
		var bub               = new BlobUriBuilder(new Uri(result.BlobUrl));

		Logger.ExecutingBlobStorageResult(result);

		if (HttpMethods.IsHead(context.HttpContext.Request.Method))
		{
			// if values are not set, pull them from blob properties
			if (result.ContentLength is null || result.LastModified is null || result.EntityTag is null)
			{
				// Get the properties of the blob
				var response   = await _client.GetPropertiesAsync(cancellationToken: cancellationToken);
				var properties = response.Value;
				result.ContentLength ??= properties.ContentLength;
				result.LastModified  ??= properties.LastModified;
				result.EntityTag     ??= MakeEtag(properties.ETag);
			}

			SetHeadersAndLog(context: context,
			                 result: result,
			                 fileLength: result.ContentLength,
			                 enableRangeProcessing: result.EnableRangeProcessing,
			                 lastModified: result.LastModified,
			                 etag: result.EntityTag);
		}
		else
		{
			// if values are not set, pull them from blob properties
			if (result.GetPropertiesBeforeDownload)
			{
				// Get the properties of the blob
				var arp        = await _client.GetPropertiesAsync(cancellationToken: cancellationToken);
				var properties = arp.Value;
				result.ContentLength ??= properties.ContentLength;
				result.LastModified  ??= properties.LastModified;
				result.EntityTag     ??= MakeEtag(properties.ETag);
			}

			var (range, rangeLength, serveBody) = SetHeadersAndLog(context: context,
			                                                       result: result,
			                                                       fileLength: result.ContentLength,
			                                                       enableRangeProcessing: result.EnableRangeProcessing,
			                                                       lastModified: result.LastModified,
			                                                       etag: result.EntityTag);

			if (serveBody)
			{
				var hr       = range is not null ? new HttpRange(range.From!.Value, rangeLength) : default;
				var response = await _client.DownloadStreamingAsync(hr, cancellationToken: cancellationToken);

				// if LastModified and ETag are not set, pull them from streaming result
				var body    = response.Value;
				var details = body.Details;
				if (result.LastModified is null || result.EntityTag is null)
				{
					var httpResponseHeaders = context.HttpContext.Response.GetTypedHeaders();
					httpResponseHeaders.LastModified = result.LastModified ??= details.LastModified;
					httpResponseHeaders.ETag         = result.EntityTag ??= MakeEtag(details.ETag);
				}

				var stream = body.Content;
				await using (stream)
				{
					await WriteAsync(context, stream);
				}
			}
		}
	}

	/// <summary>
	/// Write the contents of the <see cref="BlobStorageResult"/> to the response body.
	/// </summary>
	/// <param name="context">The <see cref="ActionContext"/>.</param>
	/// <param name="stream">The <see cref="Stream"/> to write.</param>
	protected virtual Task WriteAsync(ActionContext context, Stream stream)
	{
		if (context == null) throw new ArgumentNullException(nameof(context));
		if (stream == null) throw new ArgumentNullException(nameof(stream));

		return WriteFileAsync(context: context.HttpContext,
		                      fileStream: stream,
		                      range: null, // prevent seeking
		                      rangeLength: 0);
	}

	static EntityTagHeaderValue MakeEtag(ETag parameter) => new(parameter.ToString("H"));
}
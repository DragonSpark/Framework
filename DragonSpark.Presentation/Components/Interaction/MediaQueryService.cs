using BlazorPro.BlazorSize;
using Microsoft.JSInterop;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable All

namespace DragonSpark.Presentation.Components.Interaction;

// ATTRIBUTION: https://github.com/EdCharbeneau/BlazorSize/issues/98
sealed class MediaQueryService : IAsyncDisposable, IMediaQueryService
{
	readonly Lazy<Task<IJSObjectReference>> moduleTask;
	readonly List<MediaQueryCache>          mediaQueries = new();

	public MediaQueryService(IJSRuntime jsRuntime)
	{
		moduleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime
		                                                      .InvokeAsync<IJSObjectReference>("import",
		                                                                                       "./_content/BlazorPro.BlazorSize/blazorSizeMediaModule.js")
		                                                      .AsTask());
	}

	DotNetObjectReference<MediaQueryList> DotNetInstance { get; set; } = null!;

	public List<MediaQueryCache> MediaQueries => mediaQueries;

	MediaQueryCache? GetMediaQueryFromCache(string byMedia)
		=> mediaQueries?.Find((Predicate<MediaQueryCache>)(q => q.MediaRequested == byMedia));

	public void AddQuery(MediaQuery newMediaQuery)
	{
		var mediaQueryCache = GetMediaQueryFromCache(newMediaQuery.Media);
		if (mediaQueryCache == null)
		{
			mediaQueryCache = new MediaQueryCache()
			{
				MediaRequested = newMediaQuery.Media
			};
			mediaQueries.Add(mediaQueryCache);
		}

		mediaQueryCache.MediaQueries?.Add(newMediaQuery);
	}

	public async Task RemoveQuery(MediaQuery? mediaQuery)
	{
		if (mediaQuery == null)
			return;
		var mediaQueryFromCache = GetMediaQueryFromCache(mediaQuery.Media);
		if (mediaQueryFromCache == null)
			return;
		if (mediaQueryFromCache.MediaQueries == null)
			return;
		try
		{
			mediaQueryFromCache.MediaQueries.Remove(mediaQuery);
			if (mediaQueryFromCache.MediaQueries.Any())
				return;
			mediaQueries.Remove(mediaQueryFromCache);
			await (await moduleTask.Value).InvokeVoidAsync("removeMediaQuery", DotNetInstance,
			                                               mediaQuery.InternalMedia.Media);
		}
		catch (Exception) {}
	}

	public async Task Initialize(MediaQuery mediaQuery)
	{
		MediaQueryCache cache;
		if (mediaQuery?.Media != null)
		{
			cache = GetMediaQueryFromCache(mediaQuery.Media)!;
			MediaQueryCache mediaQueryCache;
			if (cache != null)
			{
				if (cache.Value == null)
				{
					if (!cache.Loading)
					{
						cache.Loading = true;
						var task = (await moduleTask.Value).InvokeAsync<MediaQueryArgs>("addMediaQueryToList",
						                                                                DotNetInstance,
						                                                                cache.MediaRequested);
						mediaQueryCache       = cache;
						mediaQueryCache.Value = await task;
						cache.Loading         = task.IsCompleted;
						var mediaQueryCache1 = cache;
						mediaQueryCache1.MediaQueries ??= new List<MediaQuery>();

						using var lease = cache.MediaQueries!.AsValueEnumerable().ToArray(ArrayPool<MediaQuery>.Shared);
						foreach (var mediaQuery1 in lease)
							mediaQuery1.MediaQueryChanged(cache.Value!);
					}
				}
				else
				{
					ValueTask<MediaQueryArgs> valueTask =
						(await moduleTask.Value).InvokeAsync<MediaQueryArgs>("getMediaQueryArgs", cache.MediaRequested);
					mediaQueryCache       = cache;
					mediaQueryCache.Value = await valueTask;
					mediaQuery.MediaQueryChanged(cache.Value);
				}
			}
		}
	}

	public async Task CreateMediaQueryList(DotNetObjectReference<MediaQueryList> dotNetObjectReference)
	{
		DotNetInstance = dotNetObjectReference;
		await (await moduleTask.Value).InvokeVoidAsync("addMediaQueryList", dotNetObjectReference);
	}

	public async ValueTask DisposeAsync()
	{
		var mediaQueryService = this;
		try
		{
			if (mediaQueryService.DotNetInstance != null)
			{
				var module = await mediaQueryService.moduleTask.Value;
				await module.InvokeVoidAsync("removeMediaQueryList", mediaQueryService.DotNetInstance);
				mediaQueryService.DotNetInstance.Dispose();
				await module.DisposeAsync();
				GC.SuppressFinalize(mediaQueryService);
				module = null;
			}
		}
		catch (Exception) {}
	}
}
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace DragonSpark.Presentation.Environment;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/68277029/10340424
/// </summary>
sealed class CloneHttpContext : IAlteration<HttpContext>
{
	public static CloneHttpContext Default { get; } = new();

	CloneHttpContext() {}

	public HttpContext Get(HttpContext parameter)
	{
		var feature = parameter.Features.Get<IHttpRequestFeature>().Verify();

		var requestHeaders = new Dictionary<string, StringValues>(feature.Headers.Count,
		                                                          StringComparer.OrdinalIgnoreCase);
		foreach (var header in feature.Headers)
		{
			requestHeaders[header.Key] = header.Value;
		}

		var requestFeature = new HttpRequestFeature
		{
			Protocol    = feature.Protocol,
			Method      = feature.Method,
			Scheme      = feature.Scheme,
			Path        = feature.Path,
			PathBase    = feature.PathBase,
			QueryString = feature.QueryString,
			RawTarget   = feature.RawTarget,
			Headers     = new HeaderDictionary(requestHeaders),
		};

		var features = new FeatureCollection();
		features.Set<IHttpRequestFeature>(requestFeature);
		features.Set<IHttpResponseFeature>(new HttpResponseFeature());
		features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(Stream.Null));

		return new DefaultHttpContext(features) { User = parameter.User };
	}
}
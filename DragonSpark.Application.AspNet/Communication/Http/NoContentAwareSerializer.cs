using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Communication.Http;

/// <summary>
///  ATTRIBUTION: https://github.com/reactiveui/refit/issues/1128#issuecomment-2256753536
/// </summary>
public sealed class NoContentAwareSerializer : IHttpContentSerializer
{
	readonly static Type                   Collection = typeof(ICollection<>);
	readonly        IHttpContentSerializer previous;

	public NoContentAwareSerializer(JsonSerializerOptions options)
		: this(new SystemTextJsonContentSerializer(options)) {}

	public NoContentAwareSerializer(IHttpContentSerializer previous) => this.previous = previous;

	/// <summary>
	/// Deserializes an object of type <typeparamref name="T"/> from an <see cref="HttpContent"/> object.
	/// <para>Handles responses that don't have a response body, such as <c>204 No Content</c>.</para>
	/// </summary>
	/// <typeparam name="T">Type of the object to serialize to.</typeparam>
	/// <param name="content">HttpContent object to deserialize.</param>
	/// <param name="cancellationToken">CancellationToken to abort the deserialization.</param>
	/// <returns>
	/// The deserialized object of type <typeparamref name="T"/>.
	/// <para>If there is no response body, the response is deserialized to an empty collection for collections or
	/// <c>null</c> for objects.</para>
	/// </returns>
	public Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
	{
		// if we expect a response body, deserialize it to its type
		if (content.Headers.ContentType is { MediaType: "application/json" })
		{
			return previous.FromHttpContentAsync<T>(content, cancellationToken);
		}

		// else return empty for collections or null for objects
		var type = typeof(T);
		return Task.FromResult(type.GetInterfaces()
		                           .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == Collection)
			                       ? (T?)Activator.CreateInstance(type)
			                       : default);
	}

	public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
		=> previous.GetFieldNameForProperty(propertyInfo);

	public HttpContent ToHttpContent<T>(T item) => previous.ToHttpContent(item);
}
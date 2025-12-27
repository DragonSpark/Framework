using DragonSpark.Model.Selection;
using Refit;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Communication.Http;

sealed class PolymorphicAwareContentSerializer : IHttpContentSerializer
{
	readonly IHttpContentSerializer _previous;
	readonly JsonSerializerOptions  _options;
	readonly ISelect<Type, Type?>   _map;

	public PolymorphicAwareContentSerializer(JsonSerializerOptions options)
		: this(new NoContentAwareSerializer(options), options, new PolymorphicTypes(options)) {}

	public PolymorphicAwareContentSerializer(IHttpContentSerializer previous, JsonSerializerOptions options,
	                                         ISelect<Type, Type?> map)
	{
		_previous = previous;
		_options  = options;
		_map      = map;
	}

	public HttpContent ToHttpContent<T>(T? item)
	{
		var previous = _previous.ToHttpContent(item);
		if (previous is JsonContent { Value: not null } content)
		{
			var type = item?.GetType();
			var map  = type is not null ? _map.Get(type) : null;
			if (map is not null)
			{
				return JsonContent.Create(content.Value, map, options: _options);
			}
		}

		return previous;
	}

	public Task<T?> FromHttpContentAsync<T>(HttpContent content,
	                                        CancellationToken cancellationToken = new CancellationToken())
		=> _previous.FromHttpContentAsync<T>(content, cancellationToken);

	public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
		=> _previous.GetFieldNameForProperty(propertyInfo);
}
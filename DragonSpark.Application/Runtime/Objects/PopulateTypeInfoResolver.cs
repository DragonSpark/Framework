using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace DragonSpark.Application.Runtime.Objects;

sealed class PopulateTypeInfoResolver : IJsonTypeInfoResolver
{
	readonly IJsonTypeInfoResolver _resolver;
	readonly Type                  _target;
	readonly IMutable<object?>     _store;

	public PopulateTypeInfoResolver(IJsonTypeInfoResolver resolver, Type target)
		: this(resolver, target, AmbientTarget.Default) {}

	public PopulateTypeInfoResolver(IJsonTypeInfoResolver resolver, Type target, IMutable<object?> store)
	{
		_resolver = resolver;
		_target   = target;
		_store    = store;
	}

	public JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		var result = _resolver.GetTypeInfo(type, options);
		if (type == _target)
		{
			switch (result?.Kind)
			{
				case JsonTypeInfoKind.Object:
				case JsonTypeInfoKind.Dictionary:
					var @delegate = result.CreateObject;
					result.CreateObject = () =>
					                      {
						                      var instance = _store.TryPop(out var item) ? item : @delegate?.Invoke();
						                      return instance.Verify();
					                      };
					break;
			}
		}

		return result;
	}
}
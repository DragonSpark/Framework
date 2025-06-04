using DragonSpark.Model.Results;
using System;
using System.Text.Json.Serialization.Metadata;

namespace DragonSpark.Application.Runtime.Objects;

sealed class Resolver : IResult<IJsonTypeInfoResolver>
{
	readonly Action<JsonTypeInfo>[] _modifiers;

	public Resolver(params Action<JsonTypeInfo>[] modifiers) => _modifiers = modifiers;

	public IJsonTypeInfoResolver Get()
	{
		var result = new DefaultJsonTypeInfoResolver();
		foreach (var modifier in _modifiers)
		{
			result.Modifiers.Add(modifier);
		}

		return result;
	}
}
using DragonSpark.Model.Selection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace DragonSpark.Application.Communication.Http;

sealed class DeterminePolymorphicType : ISelect<Type, Type?>
{
	readonly JsonSerializerOptions _options;

	public DeterminePolymorphicType(JsonSerializerOptions options) => _options = options;

	public Type? Get(Type parameter)
	{
		var type = parameter;
		while (type is not null)
		{
			if (_options.GetTypeInfo(type) is
			    { Kind: JsonTypeInfoKind.Object, PolymorphismOptions.DerivedTypes.Count: > 0 })
			{
				return type;
			}

			type = type.BaseType;
		}

		return null;
	}
}
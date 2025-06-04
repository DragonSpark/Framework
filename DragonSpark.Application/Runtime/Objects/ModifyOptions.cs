using DragonSpark.Model.Selection.Alterations;
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace DragonSpark.Application.Runtime.Objects;

public class ModifyOptions : IAlteration<JsonSerializerOptions>
{
	readonly IJsonTypeInfoResolver _resolver;

	public ModifyOptions(params Action<JsonTypeInfo>[] modifiers) : this(new Resolver(modifiers).Get()) {}

	public ModifyOptions(IJsonTypeInfoResolver resolver) => _resolver = resolver;

	public JsonSerializerOptions Get(JsonSerializerOptions parameter)
	{
		parameter.TypeInfoResolver ??= _resolver;
		return parameter;
	}
}
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Text.Json;

namespace DragonSpark.Application.Runtime.Objects;

// ATTRIBUTION: https://github.com/dotnet/runtime/issues/29538#issuecomment-1330494636
sealed class Map : ReferenceValueStore<JsonSerializerOptions, JsonSerializerOptions>, IAlteration<JsonSerializerOptions>
{
	public Map(Type target)
		: base(x =>
		       {
			       JsonSerializer.Serialize(value: 0, x);
			       return new(x)
			       {
				       TypeInfoResolver = new PopulateTypeInfoResolver(x.TypeInfoResolver.Verify(), target)
			       };
		       }) {}
}
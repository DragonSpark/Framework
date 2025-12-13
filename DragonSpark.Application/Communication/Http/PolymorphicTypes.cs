using DragonSpark.Model.Selection.Stores;
using System;
using System.Text.Json;

namespace DragonSpark.Application.Communication.Http;

sealed class PolymorphicTypes : ConcurrentTable<Type, Type?>
{
	public PolymorphicTypes(JsonSerializerOptions options) : this(new DeterminePolymorphicType(options)) {}

	public PolymorphicTypes(DeterminePolymorphicType select) : base(select.Get) {}
}
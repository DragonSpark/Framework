using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Objects.Entities.Generation;

sealed class LocateOnlyPrincipalProperty : ISelect<Memory<PropertyInfo>, PropertyInfo?>
{
	public static LocateOnlyPrincipalProperty Default { get; } = new();

	LocateOnlyPrincipalProperty() {}

	public PropertyInfo? Get(Memory<PropertyInfo> parameter) => parameter.Length == 1 ? parameter.Span[0] : default;
}
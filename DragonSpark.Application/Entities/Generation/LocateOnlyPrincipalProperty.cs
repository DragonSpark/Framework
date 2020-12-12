using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class LocateOnlyPrincipalProperty : ISelect<Memory<PropertyInfo>, PropertyInfo?>
	{
		public static LocateOnlyPrincipalProperty Default { get; } = new LocateOnlyPrincipalProperty();

		LocateOnlyPrincipalProperty() {}

		public PropertyInfo? Get(Memory<PropertyInfo> parameter) => parameter.Length == 1 ? parameter.Span[0] : default;
	}
}
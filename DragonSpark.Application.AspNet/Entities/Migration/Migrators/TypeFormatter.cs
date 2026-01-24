using DragonSpark.Text;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class TypeFormatter : IFormatter<Type>
{
	public static TypeFormatter Default { get; } = new();

	TypeFormatter() {}

	public string Get(Type parameter)
	{
		var type = Nullable.GetUnderlyingType(parameter);
		return type is not null ? $"{type.Name}?" : parameter.Name;
	}
}
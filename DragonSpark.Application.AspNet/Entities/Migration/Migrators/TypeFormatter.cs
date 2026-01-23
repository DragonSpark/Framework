using DragonSpark.Application.AspNet.Entities.Migration.Planning;
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
// TODO
sealed class PropertyRecordFormatter : Formatter<PropertyRecord>
{
	public static PropertyRecordFormatter Default { get; } = new();

	PropertyRecordFormatter() : this(TypeFormatter.Default) {}

	public PropertyRecordFormatter(IFormatter<Type> type) : base(x => $"{x.Name}: {type.Get(x.Type)}") {}
}
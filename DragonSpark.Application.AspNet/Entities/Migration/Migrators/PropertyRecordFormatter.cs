using DragonSpark.Application.AspNet.Entities.Migration.Planning;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class PropertyRecordFormatter : Formatter<PropertyRecord>
{
	public static PropertyRecordFormatter Default { get; } = new();

	PropertyRecordFormatter() : this(TypeFormatter.Default) {}

	public PropertyRecordFormatter(IFormatter<Type> type) : base(x => $"{x.Name}: {type.Get(x.Type)}") {}
}
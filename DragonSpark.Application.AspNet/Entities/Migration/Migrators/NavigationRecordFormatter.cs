using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class NavigationRecordFormatter : Formatter<NavigationRecord>
{
	public static NavigationRecordFormatter Default { get; } = new();

	NavigationRecordFormatter()
		: base(x => $"{x.Name} → {x.Type.Name} (Collection={x.IsCollection}, Dependent={x.IsOnDependent})") {}
}
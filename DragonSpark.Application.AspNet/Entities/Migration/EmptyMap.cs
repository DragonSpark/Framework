using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class EmptyMap : IMap
{
	public static EmptyMap Default { get; } = new();

	EmptyMap() {}
	
	public ValueTask Get(Stop<MapInput> parameter)
	{
		var ((_, to), _) = parameter;
		to.Context.Attach(to.Entity);
		return ValueTask.CompletedTask;
	}
}
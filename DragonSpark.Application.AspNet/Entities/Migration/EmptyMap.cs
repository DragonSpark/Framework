using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class EmptyMap : IMap
{
	public static EmptyMap Default { get; } = new();

	EmptyMap() {}
	
	public ValueTask Get(Stop<MapInput> parameter) => ValueTask.CompletedTask;
}
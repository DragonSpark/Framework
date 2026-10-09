using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class MinimumBatchSize : Instance<ushort>
{
	public static MinimumBatchSize Default { get; } = new();

	MinimumBatchSize() : base(100) {}
}
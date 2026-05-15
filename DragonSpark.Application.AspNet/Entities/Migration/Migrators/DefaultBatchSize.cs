using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class DefaultBatchSize : Instance<ushort>
{
	public static DefaultBatchSize Default { get; } = new();

	DefaultBatchSize() : base(5_000) {}
}
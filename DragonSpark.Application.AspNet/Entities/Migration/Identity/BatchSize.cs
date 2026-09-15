using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class BatchSize : Instance<ushort>
{
	public static BatchSize Default { get; } = new();

	BatchSize() : base(512) {}
}
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class DefaultChunkFactor : Instance<byte>
{
	public static DefaultChunkFactor Default { get; } = new();

	DefaultChunkFactor() : base(5) {}
}
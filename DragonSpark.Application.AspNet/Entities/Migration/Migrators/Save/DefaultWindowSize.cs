using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

public sealed class DefaultWindowSize : Instance<uint>
{
	public static DefaultWindowSize Default { get; } = new();

	DefaultWindowSize() : base(100_000) {}
}
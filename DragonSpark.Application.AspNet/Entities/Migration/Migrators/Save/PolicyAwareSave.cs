using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

sealed class PolicyAwareSave : PolicyAware<SaveInput, uint>, ISave
{
	public static PolicyAwareSave Default { get; } = new();

	PolicyAwareSave() : this(Save.Default) {}

	readonly ISave _previous;

	public PolicyAwareSave(ISave previous) : base(previous, DurableDataConnectionPolicy.Default) => _previous = previous;

	public uint? Get() => _previous.Get();
}
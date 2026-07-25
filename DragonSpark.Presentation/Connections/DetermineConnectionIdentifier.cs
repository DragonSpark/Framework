using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Connections;

sealed class DetermineConnectionIdentifier : CoalesceStructure<Guid>
{
	public DetermineConnectionIdentifier(PersistedConnectionIdentifier persisted, SetConnectionIdentifier set)
		: base(persisted, set) {}
}
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Connections;

sealed class DetermineConnectionIdentifier : CoalesceStructure<Guid>
{
	public DetermineConnectionIdentifier(PersistedConnectionIdentifier persisted, SetConnectionIdentifier set)
		: base(persisted, set) {}
}
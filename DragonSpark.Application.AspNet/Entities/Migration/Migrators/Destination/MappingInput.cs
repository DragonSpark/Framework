using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public readonly record struct MappingInput<T>(
	Workspace Workspace,
	Array<T> Page,
	EntityEntry<T> Current) where T : class
{
	public MappingInput(Workspace Workspace, T Current) : this(Workspace, Workspace.Source.Entry(Current)) {}

	public MappingInput(Workspace Workspace, EntityEntry<T> Current)
		: this(Workspace, Current.Entity.Yield().Result(), Current) {}
}
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public readonly record struct MappingInput<T>(
	IEntities Entities,
	Workspace Workspace,
	Array<T> Page,
	EntityEntry<T> Current) where T : class
{
	public MappingInput(IEntities Entities, Workspace Workspace, T Current)
		: this(Entities, Workspace, Workspace.Source.Entry(Current)) {}

	public MappingInput(IEntities Entities, Workspace Workspace, EntityEntry<T> Current)
		: this(Entities, Workspace, Current.Entity.Yield().Result(), Current) {}
}
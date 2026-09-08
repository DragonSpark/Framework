using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public readonly record struct MappingInput<T>(
	IResult<Workspace> Workspaces,
	Workspace Workspace,
	Array<T> Page,
	EntityEntry<T> Current) where T : class
{
	public MappingInput(IWorkspaces Workspaces, Workspace Workspace, T Current)
		: this(Workspaces, Workspace, Workspace.Source.Entry(Current)) {}

	public MappingInput(IWorkspaces Workspaces, Workspace Workspace, EntityEntry<T> Current)
		: this(Workspaces, Workspace, Current.Entity.Yield().Result(), Current) {}
}
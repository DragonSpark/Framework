namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IOriginAware : IWorkspaces
{
	Workspace Origin();
}
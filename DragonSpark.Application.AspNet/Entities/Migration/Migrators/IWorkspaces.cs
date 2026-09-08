using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IWorkspaces : IResult<Workspace>
{
	DatabaseFacade Database { get; }
	IModel Model { get; }
}
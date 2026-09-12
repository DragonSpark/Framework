using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;

public interface IDestination
{
	DatabaseFacade Database { get; }
	IModel Model { get; }
}
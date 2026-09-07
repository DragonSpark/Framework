using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IContexts : IResult<DbContext> // TODO: Rename Destination
{
	DatabaseFacade Database { get; }
	IModel Model { get; }
}
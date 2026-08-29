using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class Dependencies : ISelect<IEntityType, List<IEntityType>>
{
	public static Dependencies Default { get; } = new();

	Dependencies() {}

	public List<IEntityType> Get(IEntityType parameter)
		=> new(parameter.GetForeignKeys()
		                .Where(x => !x.IsOwnership)
		                .Select(x => x.PrincipalEntityType)
		                .Where(t => t.FindPrimaryKey() != null)
		                .SelectMany(x => x.GetDerivedTypes().Prepend(x))
		                .Where(x => !x.IsAbstract())
		                .Union(parameter.GetDerivedTypes().Where(x => !x.IsAbstract()))
		                .Distinct());
}
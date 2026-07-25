using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class Dependencies : ISelect<IEntityType, HashSet<IEntityType>>
{
	public static Dependencies Default { get; } = new();

	Dependencies() {}

	public HashSet<IEntityType> Get(IEntityType parameter) => parameter.GetForeignKeys()
	                                                                   .Where(x => !x.IsOwnership)
	                                                                   .Select(x => x.PrincipalEntityType)
	                                                                   .Where(t => t.FindPrimaryKey() != null)
	                                                                   .SelectMany(x => x.GetDerivedTypes().Prepend(x))
	                                                                   .Where(x => !x.IsAbstract())
	                                                                   .ToHashSet();
}
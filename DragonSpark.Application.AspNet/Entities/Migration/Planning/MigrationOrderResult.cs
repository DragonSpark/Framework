using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct MigrationOrderResult(
	Array<IEntityType> Linear,
	Array<Array<IEntityType>> Cycled,
	Array<IEntityType> All,
	Dictionary<IEntityType, HashSet<IEntityType>> Graph)
{
	public MigrationOrderResult(Array<IEntityType> Linear, Array<Array<IEntityType>> Cycled,
	                            Dictionary<IEntityType, HashSet<IEntityType>> Graph)
		: this(Linear, Cycled,
		       Linear.Open()
		             .Concat(Cycled.Open().SelectMany(x => x.Open()))
		             .Where(x => !x.ClrType.IsAbstract)
		             .ToArray(), Graph) {}
}
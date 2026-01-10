using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public readonly record struct MigrationPlanResult(
	Array<IEntityType> Linear,
	Array<Array<IEntityType>> Cycled,
	Array<IEntityType> All)
{
	public MigrationPlanResult(Array<IEntityType> Linear, Array<Array<IEntityType>> Cycled)
		: this(Linear, Cycled, Linear.Open().Concat(Cycled.Open().SelectMany(x => x.Open())).ToArray()) {}
}
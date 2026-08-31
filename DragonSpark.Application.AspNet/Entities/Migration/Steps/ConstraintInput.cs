using DragonSpark.Compose;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed record ConstraintInput(Array<IndexKey> Targets, Array<IGrouping<IndexKey, UniqueIndex>> Indexes)
{
	public ConstraintInput(Array<IndexKey> Targets, Array<UniqueIndex> Indexes)
		: this(Targets, Indexes.Open().GroupBy(i => new IndexKey(i.SchemaName, i.TableName, i.IndexName)).Result()) {}
}
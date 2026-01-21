using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Batches<TFrom, TTo> : ISelect<DetermineBatchInput, IBatch<TFrom>> where TFrom : class where TTo : class
{
	public static Batches<TFrom, TTo> Default { get; } = new();

	Batches() : this(IsIdentityEntity.Default) {}

	readonly ICondition<IEntityType> _identity;

	public Batches(ICondition<IEntityType> identity) => _identity = identity;

	public IBatch<TFrom> Get(DetermineBatchInput parameter)
	{
		var (source, _, map) = parameter;
		var           type     = source.Model.FindEntityType(A.Type<TFrom>()).Verify();
		var           identity = _identity.Get(type);
		IBatch<TFrom> batch    = identity ? new IdentityAwareBatch<TFrom, TTo>(map, type) : new Batch<TFrom, TTo>(map);
		return new ExceptionAwareBatch<TFrom, TTo>(batch);
	}
}
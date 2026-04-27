using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Processors<TFrom, TTo> : IProcessors<TFrom> where TFrom : class where TTo : class
{
	public static Processors<TFrom, TTo> Default { get; } = new();

	Processors() : this(IsIdentityEntity.Default) {}

	readonly ICondition<IEntityType> _identity;

	public Processors(ICondition<IEntityType> identity) => _identity = identity;

	public IEntityProcessor<TFrom> Get(ProcessorsInput parameter)
	{
		var (_, _, type, map) = parameter;
		var identity = _identity.Get(type);
		IEntityProcessor<TFrom> processor = identity
			                                    ? new IdentityAwareEntityProcessor<TFrom, TTo>(map, type)
			                                    : new UpsertEntities<TFrom, TTo>(map);
		return new ExceptionAwareEntityProcessor<TFrom, TTo>(processor);
	}
}
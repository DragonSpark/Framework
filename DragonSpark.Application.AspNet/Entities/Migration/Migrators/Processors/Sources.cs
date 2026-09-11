using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class Sources<TFrom, TTo> : ISelect<Contexts<TFrom>, ISource<TFrom>?> where TFrom : class where TTo : class
{
	public static Sources<TFrom, TTo> Default { get; } = new();

	Sources() : this(IsIdentityEntity.Default, IdentityExpressions.Default, A.Type<TTo>()) {}

	readonly ICondition<IEntityType>           _identity;
	readonly IConditional<IEntityType, string> _expressions;
	readonly Type                              _to;

	public Sources(ICondition<IEntityType> identity, IConditional<IEntityType, string> expressions, Type to)
	{
		_identity    = identity;
		_expressions = expressions;
		_to     = to;
	}

	public ISource<TFrom>? Get(Contexts<TFrom> parameter)
	{
		var (_, type, destination) = parameter;
		var key      = destination.FindEntityType(_to).Verify();
		var identity = _identity.Get(key);
		if (identity)
		{
			var from = _expressions.TryGet(type, out var e1)
				           ? e1
				           : type.FindPrimaryKey().Verify().Properties.Single().Name;
			var to = _expressions.TryGet(key, out var e2)
				         ? e2
				         : key.FindPrimaryKey().Verify().Properties.Single().Name;
			return new IdentityAwareSource<TFrom, TTo>(from, to);
		}

		return null;
	}
}
using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

sealed class Sources<TFrom, TTo> : ISelect<Contexts<TFrom>, ISource<TFrom>?> where TFrom : class where TTo : class
{
	public static Sources<TFrom, TTo> Default { get; } = new();

	Sources() : this(IsIdentityEntity.Default, IdentityExpressions.Default) {}

	readonly ICondition<IEntityType>           _identity;
	readonly IConditional<IEntityType, string> _expressions;

	public Sources(ICondition<IEntityType> identity, IConditional<IEntityType, string> expressions)
	{
		_identity    = identity;
		_expressions = expressions;
	}

	public ISource<TFrom>? Get(Contexts<TFrom> parameter)
	{
		var (_, destination, type) = parameter;
		var key      = destination.Set<TTo>().EntityType;
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
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

sealed class IdentityAwareSource<TFrom, TTo> : ISource<TFrom> where TFrom : class where TTo : class
{
	readonly ISource<TFrom> _previous;
	readonly string                _where;

	public IdentityAwareSource(IEntityType type)
		: this(Source<TFrom>.Default, $"{type.FindPrimaryKey().Verify().Properties.Single().Name} > @0") {}

	public IdentityAwareSource(ISource<TFrom> previous, string where)
	{
		_previous = previous;
		_where    = where;
	}

	public IQueryable<TFrom> Get(Stop<SourceInput<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		var set    = subject.Destination.Set<TTo>();
		var max    = set.Max(set.EntityType.FindPrimaryKey().Verify().Properties.Single().Name).Account() ?? 0;
		var input  = subject with { From = subject.From.Where(_where, max) }; // TODO: Make this an expression (faster)
		var result = _previous.Get(input.Stop(stop));
		return result;
	}
}
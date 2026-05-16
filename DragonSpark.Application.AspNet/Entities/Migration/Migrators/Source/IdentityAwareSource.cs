using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

sealed class IdentityAwareSource<TFrom, TTo> : ISource<TFrom> where TFrom : class where TTo : class
{
	readonly ISource<TFrom> _previous;
	readonly string         _column;
	readonly string         _where;

	public IdentityAwareSource(string from, string to) : this(Source<TFrom>.Default, to, $"{from} > @0") {}

	public IdentityAwareSource(ISource<TFrom> previous, string column, string where)
	{
		_previous = previous;
		_column   = column;
		_where    = where;
	}

	public IQueryable<TFrom> Get(Stop<SourceInput<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		var max    = subject.Destination.Set<TTo>().Max(_column).Account() ?? 0;
		var input  = subject with { From = subject.From.Where(_where, max) };
		var result = _previous.Get(input.Stop(stop));
		return result;
	}
}
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

sealed class IdentityAwareSource<TFrom, TTo> : ISource<TFrom> where TFrom : class where TTo : class
{
	readonly ISource<TFrom>              _previous;
	readonly ISelect<DbSet<TTo>, object> _maximum;
	readonly string                      _where;

	public IdentityAwareSource(string from, string to)
		: this(Source<TFrom>.Default, new Maximum<TTo>(to), $"{from} > @0") {}

	public IdentityAwareSource(ISource<TFrom> previous, ISelect<DbSet<TTo>, object> maximum, string where)
	{
		_previous     = previous;
		_maximum = maximum;
		_where        = where;
	}

	public IQueryable<TFrom> Get(Stop<SourceInput<TFrom>> parameter)
	{
		var (subject, stop) = parameter;
		var max    = _maximum.Get(subject.Workspace.Destination.Set<TTo>());
		var input  = subject with { Source = subject.Source.Where(_where, max) };
		var result = _previous.Get(input.Stop(stop));
		return result;
	}
}
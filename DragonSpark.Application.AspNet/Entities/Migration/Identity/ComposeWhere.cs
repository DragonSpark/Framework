using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ComposeWhere<T> : ISelect<ComposeContainsInput, Expression<Func<T, bool>>>
{
	public static ComposeWhere<T> Default { get; } = new();

	ComposeWhere() : this(ComposeContains<T>.Default, ComposeCompositeContains<T>.Default) {}

	readonly ISelect<ComposeContainsInput, Expression<Func<T, bool>>> _single;
	readonly ISelect<ComposeContainsInput, Expression<Func<T, bool>>> _composite;

	public ComposeWhere(ISelect<ComposeContainsInput, Expression<Func<T, bool>>> single,
	                    ISelect<ComposeContainsInput, Expression<Func<T, bool>>> composite)
	{
		_single    = single;
		_composite = composite;
	}

	public Expression<Func<T, bool>> Get(ComposeContainsInput parameter)
	{
		var select = parameter.Metadada.FindPrimaryKey().Verify().Properties.Count == 1 ? _single : _composite;
		return select.Get(parameter);
	}
}
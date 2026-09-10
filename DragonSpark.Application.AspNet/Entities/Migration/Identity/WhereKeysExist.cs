using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class WhereKeysExist<T> : ISelect<IEntityType, Expression<Func<T, bool>>>
{
	readonly Array<object>                                            _keys;
	readonly ISelect<ComposeContainsInput, Expression<Func<T, bool>>> _where;

	public WhereKeysExist(Array<object> keys) : this(keys, ComposeWhere<T>.Default) {}

	public WhereKeysExist(Array<object> keys, ISelect<ComposeContainsInput, Expression<Func<T, bool>>> where)
	{
		_keys  = keys;
		_where = where;
	}

	public Expression<Func<T, bool>> Get(IEntityType parameter) => _where.Get(new(parameter, _keys));
}
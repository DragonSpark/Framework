using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Components.Validation.Expressions;

[UsedImplicitly]
public class ValidatingValue<T> : Depending<T>, IValidatingValue<T>
{
	protected ValidatingValue(ISelect<T, ValueTask<bool>> @select) : base(@select) {}

	protected ValidatingValue(Func<T, ValueTask<bool>> @select) : base(@select) {}
}

public class ValidatingValue<TFrom, TTo> : IValidatingValue<TFrom>
{
	readonly IDepending<TTo>  _existing;
	readonly Func<TFrom, TTo> _select;

	protected ValidatingValue(IDepending<TTo> existing, Func<TFrom, TTo> select)
	{
		_existing = existing;
		_select   = @select;
	}

	public async ValueTask<bool> Get(TFrom parameter)
	{
		var select = _select(parameter);
		var result  = await _existing.Await(select);
		return result;
	}
}
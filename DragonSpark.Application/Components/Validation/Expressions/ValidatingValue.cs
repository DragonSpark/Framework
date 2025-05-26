using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Components.Validation.Expressions;

[UsedImplicitly]
public class ValidatingValue<T> : Depending<Stop<T>>, IValidatingValue<T>
{
	protected ValidatingValue(ISelect<Stop<T>, ValueTask<bool>> @select) : base(@select) {}

	protected ValidatingValue(Func<Stop<T>, ValueTask<bool>> @select) : base(@select) {}
}

public class ValidatingValue<TFrom, TTo> : IValidatingValue<TFrom>
{
	readonly ISelect<Stop<TTo>, ValueTask<bool>> _existing;
	readonly Func<TFrom, TTo>                    _select;

	protected ValidatingValue(ISelect<Stop<TTo>, ValueTask<bool>> existing, Func<TFrom, TTo> select)
	{
		_existing = existing;
		_select   = @select;
	}

	public async ValueTask<bool> Get(Stop<TFrom> parameter)
	{
		var select = _select(parameter);
		var result  = await _existing.Off(new(select, parameter));
		return result;
	}
}
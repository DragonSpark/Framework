using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Components.Validation.Expressions;

public class ValidatingValue<T> : Depending<T>, IValidatingValue<T>
{
	protected ValidatingValue(ISelect<T, ValueTask<bool>> @select) : base(@select) {}

	protected ValidatingValue(Func<T, ValueTask<bool>> @select) : base(@select) {}
}

public class ValidatingValue<TFrom, TTo> : IValidatingValue<TFrom>
{
	readonly IDepending<TTo>  _existing;
	readonly Func<TFrom, TTo> _select;

	public ValidatingValue(IDepending<TTo> existing, Func<TFrom, TTo> select)
	{
		_existing = existing;
		_select   = @select;
	}

	public ValueTask<bool> Get(TFrom parameter) => _existing.Get(_select(parameter));
}
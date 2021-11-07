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
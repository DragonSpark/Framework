using DragonSpark.Application.Diagnostics;
using DragonSpark.Model.Selection.Alterations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class CompensationAwareExceptions : IExceptions
{
	readonly IExceptions      _previous;
	readonly Alter<Exception> _alter;

	public CompensationAwareExceptions(IExceptions previous) : this(previous, ExceptionCompensations.Default.Get) {}

	public CompensationAwareExceptions(IExceptions previous, Alter<Exception> alter)
	{
		_previous = previous;
		_alter    = alter;
	}

	public ValueTask Get(ExceptionInput parameter)
	{
		var (owner, exception) = parameter;
		var result = _previous.Get(new (owner, _alter(exception)));
		return result;
	}
}
using DragonSpark.Application.Diagnostics;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class NavigationAwareExceptions : IExceptions
{
	readonly IExceptions      _previous;
	readonly Alter<Exception> _select;

	public NavigationAwareExceptions(IExceptions previous) : this(previous, Aggregation.Default.Get) {}

	public NavigationAwareExceptions(IExceptions previous, Alter<Exception> select)
	{
		_previous = previous;
		_select   = @select;
	}

	public ValueTask Get(ExceptionInput parameter)
	{
		var (_, exception) = parameter;
		var select = _select(exception);
		if (select is NavigationException navigation)
		{
			throw navigation;
		}

		return _previous.Get(parameter);
	}
}
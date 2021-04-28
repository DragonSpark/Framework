using DragonSpark.Application.Diagnostics;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics
{
	sealed class NavigationAwareExceptions : IExceptions
	{
		readonly IExceptions _previous;

		public NavigationAwareExceptions(IExceptions previous) => _previous = previous;

		public ValueTask Get((Type Owner, Exception Exception) parameter)
		{
			var (_, exception) = parameter;
			if (exception is NavigationException navigation)
			{
				throw navigation;
			}
			return _previous.Get(parameter);
		}
	}
}